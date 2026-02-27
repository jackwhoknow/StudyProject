#ifndef GEOMETRY_UTILS_H
#define GEOMETRY_UTILS_H

#include "GeometryBase.h"

// 辅助函数：创建矩形多边形
Polygon_2 createRectangle(double x1, double y1, double x2, double y2);

// 辅助函数：创建三角形多边形
Polygon_2 createTriangle(double x1, double y1, double x2, double y2, double x3, double y3);

#endif // GEOMETRY_UTILS_H
