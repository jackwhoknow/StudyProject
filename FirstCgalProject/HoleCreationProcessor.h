#ifndef HOLE_CREATION_PROCESSOR_H
#define HOLE_CREATION_PROCESSOR_H

#include "PolygonWithHoles.h"
#include <vector>
#include <string>
#include <memory>

// 孔洞创建处理器
class HoleCreationProcessor {
private:
    std::vector<PolygonWithHoles> polygons_with_holes_;
    std::vector<std::shared_ptr<CircleGeometry>> circles_;
    int svg_width_;
    int svg_height_;

public:
    HoleCreationProcessor(int width = 800, int height = 600);

    // 添加带孔的多边形
    void addPolygonWithHoles(const PolygonWithHoles& poly_wh);

    // 添加参考圆（仅用于显示，不创建孔洞）
    void addReferenceCircle(std::shared_ptr<CircleGeometry> circle);

    // 计算所有几何图形的边界框
    void calculateBounds(double& min_x, double& max_x, double& min_y, double& max_y);

    // 计算变换参数
    void computeTransform(double& scale, double& offset_x, double& offset_y, double padding = 50);

    // 保存到SVG文件
    void saveToSVG(const std::string& filename);

    // 打印统计信息
    void printStatistics() const;
};

#endif // HOLE_CREATION_PROCESSOR_H
