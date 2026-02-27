#include "PolygonGeometry.h"
#include <cmath>

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
    svg << "stroke-width=\"" << stroke_width_ << "\"/\u003e\n";
}
