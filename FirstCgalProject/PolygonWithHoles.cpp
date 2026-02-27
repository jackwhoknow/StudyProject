#include "PolygonWithHoles.h"

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
