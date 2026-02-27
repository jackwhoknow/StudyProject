#include "Geometry.h"
#include <cmath>

#ifndef M_PI
#define M_PI 3.14159265358979323846
#endif

// PolygonGeometry implementation
PolygonGeometry::PolygonGeometry(const Polygon_2& poly, 
                                const std::string& fill,
                                const std::string& stroke,
                                int stroke_width)
    : polygon_(poly), fill_color_(fill), stroke_color_(stroke), stroke_width_(stroke_width) {}

void PolygonGeometry::getBounds(double& min_x, double& max_x, double& min_y, double& max_y) const {
    for (auto vit = polygon_.vertices_begin(); vit != polygon_.vertices_end(); ++vit) {
        double x = CGAL::to_double(vit->x());
        double y = CGAL::to_double(vit->y());
        min_x = std::min(min_x, x);
        max_x = std::max(max_x, x);
        min_y = std::min(min_y, y);
        max_y = std::max(max_y, y);
    }
}

void PolygonGeometry::transformAndDraw(std::ofstream& svg, double scale, double offset_x, double offset_y, int height) const {
    if (polygon_.is_empty()) return;
    
    svg << "  <polygon points=\"";
    for (auto vit = polygon_.vertices_begin(); vit != polygon_.vertices_end(); ++vit) {
        double x = CGAL::to_double(vit->x());
        double y = CGAL::to_double(vit->y());
        double px = x * scale + offset_x;
        double py = height - (y * scale + offset_y);
        svg << px << "," << py << " ";
    }
    svg << "\" ";
    svg << "fill=\"" << fill_color_ << "\" ";
    svg << "stroke=\"" << stroke_color_ << "\" ";
    svg << "stroke-width=\"" << stroke_width_ << "\"/>\n";
}

// CircleGeometry implementation
CircleGeometry::CircleGeometry(const Point_2& center, double radius, int num_segments,
                               const std::string& fill,
                               const std::string& stroke,
                               int stroke_width)
    : center_(center), radius_(radius), num_segments_(num_segments),
      fill_color_(fill), stroke_color_(stroke), stroke_width_(stroke_width) {}

Polygon_2 CircleGeometry::toPolygon() const {
    Polygon_2 poly;
    double cx = CGAL::to_double(center_.x());
    double cy = CGAL::to_double(center_.y());
    
    for (int i = 0; i < num_segments_; ++i) {
        double angle = 2 * M_PI * i / num_segments_;
        double x = cx + radius_ * cos(angle);
        double y = cy + radius_ * sin(angle);
        poly.push_back(Point_2(x, y));
    }
    return poly;
}

void CircleGeometry::getBounds(double& min_x, double& max_x, double& min_y, double& max_y) const {
    double cx = CGAL::to_double(center_.x());
    double cy = CGAL::to_double(center_.y());
    min_x = std::min(min_x, cx - radius_);
    max_x = std::max(max_x, cx + radius_);
    min_y = std::min(min_y, cy - radius_);
    max_y = std::max(max_y, cy + radius_);
}

void CircleGeometry::transformAndDraw(std::ofstream& svg, double scale, double offset_x, double offset_y, int height) const {
    double cx = CGAL::to_double(center_.x());
    double cy = CGAL::to_double(center_.y());
    double px = cx * scale + offset_x;
    double py = height - (cy * scale + offset_y);
    double r = radius_ * scale;
    
    svg << "  <circle cx=\"" << px << "\" cy=\"" << py << "\" r=\"" << r << "\" ";
    svg << "fill=\"" << fill_color_ << "\" ";
    svg << "stroke=\"" << stroke_color_ << "\" ";
    svg << "stroke-width=\"" << stroke_width_ << "\"/>\n";
}

// PolygonWithHoles implementation
void PolygonWithHoles::addHole(std::shared_ptr<PolygonGeometry> hole) {
    holes_.push_back(hole);
}

void PolygonWithHoles::addCircleHole(std::shared_ptr<CircleGeometry> circle) {
    circle_holes_.push_back(circle);
}

void PolygonWithHoles::getAllGeometries(std::vector<std::shared_ptr<Geometry>>& geometries) const {
    geometries.push_back(outer_);
    for (const auto& hole : holes_) {
        geometries.push_back(hole);
    }
    for (const auto& circle : circle_holes_) {
        geometries.push_back(circle);
    }
}

void PolygonWithHoles::drawToSVG(std::ofstream& svg, double scale, double offset_x, double offset_y, int height) const {
    // Draw outer boundary
    outer_->transformAndDraw(svg, scale, offset_x, offset_y, height);
    
    // Draw polygon holes (white fill)
    for (const auto& hole : holes_) {
        PolygonGeometry white_hole(hole->getPolygon(), "white", "blue", 2);
        white_hole.transformAndDraw(svg, scale, offset_x, offset_y, height);
    }
    
    // Draw circular holes (convert to polygon then draw)
    for (const auto& circle : circle_holes_) {
        Polygon_2 circle_poly = circle->toPolygon();
        PolygonGeometry white_hole(circle_poly, "white", "blue", 2);
        white_hole.transformAndDraw(svg, scale, offset_x, offset_y, height);
    }
}

// HoleCreationProcessor implementation
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
    svg << "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
    svg << "<svg width=\"" << svg_width_ << "\" height=\"" << svg_height_ 
        << "\" xmlns=\"http://www.w3.org/2000/svg\">\n";
    svg << "  <rect width=\"" << svg_width_ << "\" height=\"" << svg_height_ 
        << "\" fill=\"white\"/>\n";
    svg << "  <text x=\"" << svg_width_/2 << "\" y=\"30\" text-anchor=\"middle\" "
        << "font-size=\"20\" fill=\"black\">CGAL Hole Creation</text>\n";

    // Draw all polygons with holes
    svg << "  <!-- Polygons with holes -->\n";
    for (const auto& poly_wh : polygons_with_holes_) {
        poly_wh.drawToSVG(svg, scale, offset_x, offset_y, svg_height_);
    }

    // Draw reference circles (outline only)
    svg << "  <!-- Reference circles -->\n";
    for (const auto& circle : circles_) {
        CircleGeometry outline(circle->getCenter(), circle->getRadius(), 32, "none", "red", 1);
        outline.transformAndDraw(svg, scale, offset_x, offset_y, svg_height_);
    }

    svg << "</svg>\n";
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

// Helper functions
Polygon_2 createRectangle(double x1, double y1, double x2, double y2) {
    Polygon_2 poly;
    poly.push_back(Point_2(x1, y1));
    poly.push_back(Point_2(x2, y1));
    poly.push_back(Point_2(x2, y2));
    poly.push_back(Point_2(x1, y2));
    return poly;
}

Polygon_2 createTriangle(double x1, double y1, double x2, double y2, double x3, double y3) {
    Polygon_2 poly;
    poly.push_back(Point_2(x1, y1));
    poly.push_back(Point_2(x2, y2));
    poly.push_back(Point_2(x3, y3));
    return poly;
}
