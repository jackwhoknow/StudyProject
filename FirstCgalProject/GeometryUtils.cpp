#include "GeometryUtils.h"

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
