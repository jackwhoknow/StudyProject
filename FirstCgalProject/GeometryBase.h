#ifndef GEOMETRY_BASE_H
#define GEOMETRY_BASE_H

#include <CGAL/Exact_predicates_inexact_constructions_kernel.h>
#include <CGAL/Polygon_2.h>
#include <fstream>

typedef CGAL::Exact_predicates_inexact_constructions_kernel K;
typedef K::Point_2 Point_2;
typedef CGAL::Polygon_2<K> Polygon_2;

// 几何基类
class Geometry {
public:
    virtual ~Geometry() = default;
    virtual void getBounds(double& min_x, double& max_x, double& min_y, double& max_y) const = 0;
    virtual void transformAndDraw(std::ofstream& svg, double scale, double offset_x, double offset_y, int height) const = 0;
};

#endif // GEOMETRY_BASE_H
