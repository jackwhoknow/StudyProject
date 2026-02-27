#ifndef GEOMETRY_H
#define GEOMETRY_H

#include <CGAL/Exact_predicates_inexact_constructions_kernel.h>
#include <CGAL/Polygon_2.h>
#include <vector>
#include <iostream>
#include <fstream>
#include <memory>

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

// 多边形类
class PolygonGeometry : public Geometry {
private:
    Polygon_2 polygon_;
    std::string fill_color_;
    std::string stroke_color_;
    int stroke_width_;

public:
    PolygonGeometry(const Polygon_2& poly, 
                    const std::string& fill = "lightblue",
                    const std::string& stroke = "blue",
                    int stroke_width = 2);

    const Polygon_2& getPolygon() const { return polygon_; }
    
    void getBounds(double& min_x, double& max_x, double& min_y, double& max_y) const override;
    void transformAndDraw(std::ofstream& svg, double scale, double offset_x, double offset_y, int height) const override;
};

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

// 带洞多边形类
class PolygonWithHoles {
private:
    std::shared_ptr<PolygonGeometry> outer_;
    std::vector<std::shared_ptr<PolygonGeometry>> holes_;
    std::vector<std::shared_ptr<CircleGeometry>> circle_holes_;

public:
    PolygonWithHoles(std::shared_ptr<PolygonGeometry> outer) : outer_(outer) {}

    // 添加多边形洞
    void addHole(std::shared_ptr<PolygonGeometry> hole);

    // 添加圆形洞
    void addCircleHole(std::shared_ptr<CircleGeometry> circle);

    // 获取所有几何体用于计算边界
    void getAllGeometries(std::vector<std::shared_ptr<Geometry>>& geometries) const;

    // 绘制到SVG
    void drawToSVG(std::ofstream& svg, double scale, double offset_x, double offset_y, int height) const;

    size_t getHoleCount() const { return holes_.size() + circle_holes_.size(); }
};

// 造洞处理器类
class HoleCreationProcessor {
private:
    std::vector<PolygonWithHoles> polygons_with_holes_;
    std::vector<std::shared_ptr<CircleGeometry>> circles_;
    int svg_width_;
    int svg_height_;

public:
    HoleCreationProcessor(int width = 800, int height = 600);

    // 添加带洞的多边形
    void addPolygonWithHoles(const PolygonWithHoles& poly_wh);

    // 添加参考圆（仅用于显示，不参与造洞）
    void addReferenceCircle(std::shared_ptr<CircleGeometry> circle);

    // 计算所有几何体的边界框
    void calculateBounds(double& min_x, double& max_x, double& min_y, double& max_y);

    // 计算变换参数
    void computeTransform(double& scale, double& offset_x, double& offset_y, double padding = 50);

    // 保存到SVG文件
    void saveToSVG(const std::string& filename);

    // 打印统计信息
    void printStatistics() const;
};

// 辅助函数：创建矩形多边形
Polygon_2 createRectangle(double x1, double y1, double x2, double y2);

// 辅助函数：创建三角形多边形
Polygon_2 createTriangle(double x1, double y1, double x2, double y2, double x3, double y3);

#endif // GEOMETRY_H
