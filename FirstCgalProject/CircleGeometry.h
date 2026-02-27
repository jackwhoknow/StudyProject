#ifndef CIRCLE_GEOMETRY_H
#define CIRCLE_GEOMETRY_H

#include "GeometryBase.h"
#include <string>

// 圆形类
class CircleGeometry : public Geometry {
private:
    Point_2 center_;
    double radius_;
    int num_segments_;
    std::string fill_color_;
    std::string stroke_color_;
    int stroke_width_;

public:
    CircleGeometry(const Point_2& center, double radius, int num_segments = 32,
                   const std::string& fill = "lightcoral",
                   const std::string& stroke = "red",
                   int stroke_width = 2);

    Point_2 getCenter() const { return center_; }
    double getRadius() const { return radius_; }
    
    // 将圆转换为多边形
    Polygon_2 toPolygon() const;

    void getBounds(double& min_x, double& max_x, double& min_y, double& max_y) const override;
    void transformAndDraw(std::ofstream& svg, double scale, double offset_x, double offset_y, int height) const override;
};

#endif // CIRCLE_GEOMETRY_H
