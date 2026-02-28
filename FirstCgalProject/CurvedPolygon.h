#ifndef CURVED_POLYGON_H
#define CURVED_POLYGON_H

#include "GeometryBase.h"
#include <vector>
#include <memory>

// 边类型枚举
enum class EdgeType {
    LINE_SEGMENT,    // 直线段
    CIRCULAR_ARC     // 圆弧
};

// 边基类
class Edge {
public:
    virtual ~Edge() = default;
    virtual EdgeType getType() const = 0;
    virtual Point_2 getStart() const = 0;
    virtual Point_2 getEnd() const = 0;
    virtual double length() const = 0;
    virtual Point_2 pointAt(double t) const = 0;  // t in [0,1]
    virtual double distanceToPoint(const Point_2& point) const = 0;
    virtual bool containsPoint(const Point_2& point, double tolerance = 1e-9) const = 0;
};

// 直线段边
class LineEdge : public Edge {
private:
    Point_2 start_;
    Point_2 end_;

public:
    LineEdge(const Point_2& start, const Point_2& end);
    
    EdgeType getType() const override { return EdgeType::LINE_SEGMENT; }
    Point_2 getStart() const override { return start_; }
    Point_2 getEnd() const override { return end_; }
    double length() const override;
    Point_2 pointAt(double t) const override;
    double distanceToPoint(const Point_2& point) const override;
    bool containsPoint(const Point_2& point, double tolerance = 1e-9) const override;
    
    // 获取直线方向
    K::Vector_2 getDirection() const;
    // 获取直线方程
    K::Line_2 getLine() const;
};

// 圆弧边
class ArcEdge : public Edge {
private:
    Point_2 start_;      // 起点
    Point_2 end_;        // 终点
    Point_2 mid_;        // 中点（用于定义圆弧的弯曲方向）
    Point_2 center_;     // 圆心（缓存）
    double radius_;      // 半径（缓存）
    bool clockwise_;     // 是否顺时针
    
    void computeCircle();  // 计算圆心和半径

public:
    ArcEdge(const Point_2& start, const Point_2& end, const Point_2& mid);
    
    EdgeType getType() const override { return EdgeType::CIRCULAR_ARC; }
    Point_2 getStart() const override { return start_; }
    Point_2 getEnd() const override { return end_; }
    Point_2 getMid() const { return mid_; }
    Point_2 getCenter() const { return center_; }
    double getRadius() const { return radius_; }
    bool isClockwise() const { return clockwise_; }
    
    double length() const override;
    Point_2 pointAt(double t) const override;
    double distanceToPoint(const Point_2& point) const override;
    bool containsPoint(const Point_2& point, double tolerance = 1e-9) const override;
    
    // 获取圆弧角度范围（弧度）
    void getAngleRange(double& startAngle, double& endAngle) const;
    // 判断角度是否在圆弧范围内
    bool angleInRange(double angle) const;
};

// 支持圆弧的多边形类
class CurvedPolygon {
private:
    std::vector<std::shared_ptr<Edge>> edges_;
    bool closed_;
    
public:
    CurvedPolygon() : closed_(false) {}
    
    // 添加直线段
    void addLineSegment(const Point_2& start, const Point_2& end);
    
    // 添加圆弧（通过起点、终点和中点）
    void addArc(const Point_2& start, const Point_2& end, const Point_2& mid);
    
    // 添加圆弧（通过起点、终点、圆心和方向）
    void addArc(const Point_2& start, const Point_2& end, const Point_2& center, bool clockwise);
    
    // 关闭多边形（连接最后一点到第一点）
    void close();
    
    // 判断是否已关闭
    bool isClosed() const { return closed_; }
    
    // 获取边数
    size_t edgeCount() const { return edges_.size(); }
    
    // 获取边
    const Edge& getEdge(size_t index) const { return *edges_[index]; }
    std::shared_ptr<Edge> getEdgePtr(size_t index) const { return edges_[index]; }
    
    // 获取所有边
    const std::vector<std::shared_ptr<Edge>>& getEdges() const { return edges_; }
    
    // 获取顶点数
    size_t vertexCount() const;
    
    // 获取顶点
    Point_2 getVertex(size_t index) const;
    
    // 计算周长
    double perimeter() const;
    
    // 计算面积（使用格林公式）
    double area() const;
    
    // 判断点是否在多边形内（射线法）
    bool containsPoint(const Point_2& point) const;
    
    // 判断点是否在多边形边上
    bool pointOnBoundary(const Point_2& point, double tolerance = 1e-9) const;
    
    // 判断点与多边形的关系
    // 返回值：-1=外部, 0=边上, 1=内部
    int pointRelation(const Point_2& point, double tolerance = 1e-9) const;
    
    // 获取点到多边形的距离
    double distanceToPoint(const Point_2& point) const;
    
    // 判断是否为简单多边形（无自交）
    bool isSimple() const;
    
    // 获取包围盒
    void getBoundingBox(double& minX, double& maxX, double& minY, double& maxY) const;
    
    // 清空多边形
    void clear();
    
    // 转换为普通Polygon_2（仅含直线段近似）
    Polygon_2 toPolygon(int arcSegments = 32) const;
};

#endif // CURVED_POLYGON_H
