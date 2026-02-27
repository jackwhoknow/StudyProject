#ifndef POLYGON_GEOMETRY_H
#define POLYGON_GEOMETRY_H

#include "GeometryBase.h"
#include <string>

// ∂‡±ﬂ–Œ¿‡
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

#endif // POLYGON_GEOMETRY_H
