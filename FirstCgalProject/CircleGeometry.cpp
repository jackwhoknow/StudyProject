#include "CircleGeometry.h"
#include <cmath>

#ifndef M_PI
#define M_PI 3.14159265358979323846
#endif

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
    svg << "stroke-width=\"" << stroke_width_ << "\"/\u003e\n";
}
