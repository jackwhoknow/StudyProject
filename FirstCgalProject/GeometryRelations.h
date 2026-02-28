#ifndef GEOMETRY_RELATIONS_H
#define GEOMETRY_RELATIONS_H

#include "GeometryBase.h"
#include <CGAL/Polygon_2_algorithms.h>
#include <CGAL/Kernel/global_functions.h>
#include <CGAL/enum.h>

// 点与多边形位置关系枚举
enum class PointPolygonRelation {
    INSIDE,     // 点在多边形内部
    ON_EDGE,    // 点在多边形边上
    OUTSIDE     // 点在多边形外部
};

// 点与直线/线段位置关系枚举
enum class PointLineRelation {
    ON_LINE,        // 点在直线上
    ON_SEGMENT,     // 点在线段上（包含端点）
    ON_SEGMENT_INTERIOR, // 点在线段内部（不包含端点）
    LEFT_OF_LINE,   // 点在直线左侧
    RIGHT_OF_LINE,  // 点在直线右侧
    OUTSIDE_SEGMENT // 点在直线延长线上，但不在线段范围内
};

// 几何关系判断类
class GeometryRelations {
public:
    // ========== 点与多边形关系 ==========
    
    // 判断点与多边形的位置关系
    static PointPolygonRelation pointInPolygon(const Point_2& point, const Polygon_2& poly);
    
    // 判断点是否在多边形内部（不包含边界）
    static bool isPointInsidePolygon(const Point_2& point, const Polygon_2& poly);
    
    // 判断点是否在多边形边上（包含顶点）
    static bool isPointOnPolygonEdge(const Point_2& point, const Polygon_2& poly, double tolerance = 1e-9);
    
    // 判断点是否在多边形外部
    static bool isPointOutsidePolygon(const Point_2& point, const Polygon_2& poly);
    
    // 获取点到多边形的带符号距离（内部为负，外部为正，边上为零）
    static double signedDistanceToPolygon(const Point_2& point, const Polygon_2& poly);
    
    // 获取点到多边形边的最小距离
    static double distanceToPolygonEdge(const Point_2& point, const Polygon_2& poly);
    
    // ========== 点与直线/线段关系 ==========
    
    // 判断点与直线的位置关系
    static PointLineRelation pointToLineRelation(const Point_2& point, 
                                                  const Point_2& lineStart, 
                                                  const Point_2& lineEnd);
    
    // 判断点是否在直线上
    static bool isPointOnLine(const Point_2& point, 
                             const Point_2& lineStart, 
                             const Point_2& lineEnd, 
                             double tolerance = 1e-9);
    
    // 判断点是否在线段上（包含端点）
    static bool isPointOnSegment(const Point_2& point, 
                                const Point_2& segStart, 
                                const Point_2& segEnd, 
                                double tolerance = 1e-9);
    
    // 判断点是否在线段内部（不包含端点）
    static bool isPointOnSegmentInterior(const Point_2& point, 
                                        const Point_2& segStart, 
                                        const Point_2& segEnd, 
                                        double tolerance = 1e-9);
    
    // 判断点在直线的哪一侧
    static CGAL::Oriented_side pointSideOfLine(const Point_2& point, 
                                               const Point_2& lineStart, 
                                               const Point_2& lineEnd);
    
    // 获取点到直线的距离
    static double distancePointToLine(const Point_2& point, 
                                     const Point_2& lineStart, 
                                     const Point_2& lineEnd);
    
    // 获取点到线段的距离
    static double distancePointToSegment(const Point_2& point, 
                                        const Point_2& segStart, 
                                        const Point_2& segEnd);
    
    // 获取点在线段上的投影点
    static Point_2 projectPointToSegment(const Point_2& point, 
                                        const Point_2& segStart, 
                                        const Point_2& segEnd);
    
    // ========== 辅助函数 ==========
    
    // 将关系枚举转换为字符串
    static const char* pointPolygonRelationToString(PointPolygonRelation relation);
    static const char* pointLineRelationToString(PointLineRelation relation);
    
    // 打印点与多边形的关系信息
    static void printPointPolygonRelation(const Point_2& point, const Polygon_2& poly);
    
    // 打印点与线段的关系信息
    static void printPointSegmentRelation(const Point_2& point, 
                                         const Point_2& segStart, 
                                         const Point_2& segEnd);
};

#endif // GEOMETRY_RELATIONS_H
