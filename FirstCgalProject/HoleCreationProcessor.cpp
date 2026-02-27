#include "HoleCreationProcessor.h"
#include <iostream>

HoleCreationProcessor::HoleCreationProcessor(int width, int height) 
    : svg_width_(width), svg_height_(height) {}

void HoleCreationProcessor::addPolygonWithHoles(const PolygonWithHoles& poly_wh) {
    polygons_with_holes_.push_back(poly_wh);
}

void HoleCreationProcessor::addReferenceCircle(std::shared_ptr<CircleGeometry> circle) {
    circles_.push_back(circle);
}

void HoleCreationProcessor::calculateBounds(double& min_x, double& max_x, double& min_y, double& max_y) {
    min_x = 1e10; max_x = -1e10;
    min_y = 1e10; max_y = -1e10;

    // Collect bounds from all polygons with holes
    for (const auto& poly_wh : polygons_with_holes_) {
        std::vector<std::shared_ptr<Geometry>> geometries;
        poly_wh.getAllGeometries(geometries);
        for (const auto& geom : geometries) {
            geom->getBounds(min_x, max_x, min_y, max_y);
        }
    }

    // Collect bounds from reference circles
    for (const auto& circle : circles_) {
        circle->getBounds(min_x, max_x, min_y, max_y);
    }
}

void HoleCreationProcessor::computeTransform(double& scale, double& offset_x, double& offset_y, double padding) {
    double min_x, max_x, min_y, max_y;
    calculateBounds(min_x, max_x, min_y, max_y);

    double bbox_width = max_x - min_x;
    double bbox_height = max_y - min_y;
    scale = std::min((svg_width_ - 2 * padding) / bbox_width, 
                    (svg_height_ - 2 * padding) / bbox_height);
    offset_x = (svg_width_ - bbox_width * scale) / 2 - min_x * scale;
    offset_y = (svg_height_ - bbox_height * scale) / 2 - min_y * scale;
}

void HoleCreationProcessor::saveToSVG(const std::string& filename) {
    double scale, offset_x, offset_y;
    computeTransform(scale, offset_x, offset_y);

    std::ofstream svg(filename);
    svg << "<?xml version=\"1.0\" encoding=\"UTF-8\"?\u003e\n";
    svg << "<svg width=\"" << svg_width_ << "\" height=\"" << svg_height_ 
        << "\" xmlns=\"http://www.w3.org/2000/svg\"\u003e\n";
    svg << "  <rect width=\"" << svg_width_ << "\" height=\"" << svg_height_ 
        << "\" fill=\"white\"/\u003e\n";
    svg << "  <text x=\"" << svg_width_/2 << "\" y=\"30\" text-anchor=\"middle\" "
        << "font-size=\"20\" fill=\"black\"\u003eCGAL Hole Creation</text\u003e\n";

    // Draw all polygons with holes
    svg << "  <!-- Polygons with holes --\u003e\n";
    for (const auto& poly_wh : polygons_with_holes_) {
        poly_wh.drawToSVG(svg, scale, offset_x, offset_y, svg_height_);
    }

    // Draw reference circles (outline only)
    svg << "  <!-- Reference circles --\u003e\n";
    for (const auto& circle : circles_) {
        CircleGeometry outline(circle->getCenter(), circle->getRadius(), 32, "none", "red", 1);
        outline.transformAndDraw(svg, scale, offset_x, offset_y, svg_height_);
    }

    svg << "</svg\u003e\n";
    svg.close();
}

void HoleCreationProcessor::printStatistics() const {
    std::cout << "\nHole Creation Statistics:" << std::endl;
    std::cout << "=========================" << std::endl;
    std::cout << "Total polygons with holes: " << polygons_with_holes_.size() << std::endl;
    
    for (size_t i = 0; i < polygons_with_holes_.size(); ++i) {
        std::cout << "  Polygon " << (i + 1) << ": " 
                 << polygons_with_holes_[i].getHoleCount() << " holes" << std::endl;
    }
    
    std::cout << "Reference circles: " << circles_.size() << std::endl;
}
