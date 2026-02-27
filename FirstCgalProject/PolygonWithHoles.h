#ifndef POLYGON_WITH_HOLES_H
#define POLYGON_WITH_HOLES_H

#include "PolygonGeometry.h"
#include "CircleGeometry.h"
#include <vector>
#include <memory>

// 带孔多边形类
class PolygonWithHoles {
private:
    std::shared_ptr<PolygonGeometry> outer_;
    std::vector<std::shared_ptr<PolygonGeometry>> holes_;
    std::vector<std::shared_ptr<CircleGeometry>> circle_holes_;

public:
    PolygonWithHoles(std::shared_ptr<PolygonGeometry> outer) : outer_(outer) {}

    // 添加多边形孔
    void addHole(std::shared_ptr<PolygonGeometry> hole);

    // 添加圆形孔
    void addCircleHole(std::shared_ptr<CircleGeometry> circle);

    // 获取所有几何图形以计算边界
    void getAllGeometries(std::vector<std::shared_ptr<Geometry>>& geometries) const;

    // 绘制到SVG
    void drawToSVG(std::ofstream& svg, double scale, double offset_x, double offset_y, int height) const;

    size_t getHoleCount() const { return holes_.size() + circle_holes_.size(); }
};

#endif // POLYGON_WITH_HOLES_H
