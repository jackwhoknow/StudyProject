#include "CurvedPolygon.h"
#include <cmath>
#include <algorithm>
#include <limits>

#ifndef M_PI
#define M_PI 3.14159265358979323846
#endif

// ==================== Helper Functions ====================

// Convert CGAL point to double coordinates
inline void getPointCoords(const Point_2& p, double& x, double& y) {
    x = CGAL::to_double(p.x());
    y = CGAL::to_double(p.y());
}

// Compute squared distance between two points
inline double squaredDist(const Point_2& p1, const Point_2& p2) {
    double x1, y1, x2, y2;
    getPointCoords(p1, x1, y1);
    getPointCoords(p2, x2, y2);
    double dx = x2 - x1;
    double dy = y2 - y1;
    return dx * dx + dy * dy;
}

// ==================== LineEdge Implementation ====================

LineEdge::LineEdge(const Point_2& start, const Point_2& end)
    : start_(start), end_(end) {}

double LineEdge::length() const {
    return std::sqrt(squaredDist(start_, end_));
}

Point_2 LineEdge::pointAt(double t) const {
    t = std::max(0.0, std::min(1.0, t));
    double x1, y1, x2, y2;
    getPointCoords(start_, x1, y1);
    getPointCoords(end_, x2, y2);
    double x = x1 + t * (x2 - x1);
    double y = y1 + t * (y2 - y1);
    return Point_2(x, y);
}

double LineEdge::distanceToPoint(const Point_2& point) const {
    double x1, y1, x2, y2, px, py;
    getPointCoords(start_, x1, y1);
    getPointCoords(end_, x2, y2);
    getPointCoords(point, px, py);
    
    // Vector from start to end
    double dx = x2 - x1;
    double dy = y2 - y1;
    
    // Vector from start to point
    double wx = px - x1;
    double wy = py - y1;
    
    // Project point onto line
    double c1 = wx * dx + wy * dy;
    if (c1 <= 0) {
        // Closest to start point
        double dsx = px - x1;
        double dsy = py - y1;
        return std::sqrt(dsx * dsx + dsy * dsy);
    }
    
    double c2 = dx * dx + dy * dy;
    if (c2 <= c1) {
        // Closest to end point
        double dex = px - x2;
        double dey = py - y2;
        return std::sqrt(dex * dex + dey * dey);
    }
    
    // Closest to line segment
    double b = c1 / c2;
    double projX = x1 + b * dx;
    double projY = y1 + b * dy;
    double dpx = px - projX;
    double dpy = py - projY;
    return std::sqrt(dpx * dpx + dpy * dpy);
}

bool LineEdge::containsPoint(const Point_2& point, double tolerance) const {
    double x1, y1, x2, y2, px, py;
    getPointCoords(start_, x1, y1);
    getPointCoords(end_, x2, y2);
    getPointCoords(point, px, py);
    
    // Check if point is on the line (distance check)
    double dx = x2 - x1;
    double dy = y2 - y1;
    double wx = px - x1;
    double wy = py - y1;
    
    // Cross product to check collinearity
    double cross = wx * dy - wy * dx;
    if (std::abs(cross) > tolerance * std::sqrt(dx * dx + dy * dy)) {
        return false;
    }
    
    // Check if within bounding box
    double minX = std::min(x1, x2) - tolerance;
    double maxX = std::max(x1, x2) + tolerance;
    double minY = std::min(y1, y2) - tolerance;
    double maxY = std::max(y1, y2) + tolerance;
    
    return px >= minX && px <= maxX && py >= minY && py <= maxY;
}

K::Vector_2 LineEdge::getDirection() const {
    double x1, y1, x2, y2;
    getPointCoords(start_, x1, y1);
    getPointCoords(end_, x2, y2);
    return K::Vector_2(x2 - x1, y2 - y1);
}

K::Line_2 LineEdge::getLine() const {
    return K::Line_2(start_, end_);
}

// ==================== ArcEdge Implementation ====================

ArcEdge::ArcEdge(const Point_2& start, const Point_2& end, const Point_2& mid)
    : start_(start), end_(end), mid_(mid), radius_(0.0), clockwise_(false) {
    computeCircle();
}

void ArcEdge::computeCircle() {
    double x1, y1, x2, y2, x3, y3;
    getPointCoords(start_, x1, y1);
    getPointCoords(end_, x2, y2);
    getPointCoords(mid_, x3, y3);
    
    // Compute circle from 3 points using perpendicular bisector method
    double d = 2 * (x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2));
    
    if (std::abs(d) < 1e-10) {
        // Points are collinear, use midpoint as center
        double cx = (x1 + x2) / 2;
        double cy = (y1 + y2) / 2;
        center_ = Point_2(cx, cy);
        double dx = x1 - cx;
        double dy = y1 - cy;
        radius_ = std::sqrt(dx * dx + dy * dy);
        return;
    }
    
    double ux = ((x1 * x1 + y1 * y1) * (y2 - y3) + 
                 (x2 * x2 + y2 * y2) * (y3 - y1) + 
                 (x3 * x3 + y3 * y3) * (y1 - y2)) / d;
    double uy = ((x1 * x1 + y1 * y1) * (x3 - x2) + 
                 (x2 * x2 + y2 * y2) * (x1 - x3) + 
                 (x3 * x3 + y3 * y3) * (x2 - x1)) / d;
    
    center_ = Point_2(ux, uy);
    double dx = x1 - ux;
    double dy = y1 - uy;
    radius_ = std::sqrt(dx * dx + dy * dy);
    
    // Determine direction (clockwise or counter-clockwise)
    double cx, cy;
    getPointCoords(center_, cx, cy);
    
    // Vectors from center to points
    double v1x = x1 - cx;
    double v1y = y1 - cy;
    double v2x = x3 - cx;
    double v2y = y3 - cy;
    double v3x = x2 - cx;
    double v3y = y2 - cy;
    
    // Cross products to determine orientation
    double cross1 = v1x * v2y - v1y * v2x;
    double cross2 = v2x * v3y - v2y * v3x;
    
    clockwise_ = (cross1 > 0) != (cross2 > 0);
    if (cross1 > 0 && cross2 > 0) {
        clockwise_ = false;
    } else if (cross1 < 0 && cross2 < 0) {
        clockwise_ = true;
    }
}

double ArcEdge::length() const {
    double startAngle, endAngle;
    getAngleRange(startAngle, endAngle);
    
    double angleDiff = endAngle - startAngle;
    if (angleDiff < 0) {
        angleDiff += 2 * M_PI;
    }
    
    if (clockwise_) {
        angleDiff = 2 * M_PI - angleDiff;
    }
    
    return radius_ * angleDiff;
}

Point_2 ArcEdge::pointAt(double t) const {
    t = std::max(0.0, std::min(1.0, t));
    
    double startAngle, endAngle;
    getAngleRange(startAngle, endAngle);
    
    double angle;
    if (clockwise_) {
        angle = startAngle - t * (startAngle - endAngle + 2 * M_PI);
        if (angle < 0) angle += 2 * M_PI;
    } else {
        double angleDiff = endAngle - startAngle;
        if (angleDiff < 0) angleDiff += 2 * M_PI;
        angle = startAngle + t * angleDiff;
    }
    
    double cx, cy;
    getPointCoords(center_, cx, cy);
    double x = cx + radius_ * std::cos(angle);
    double y = cy + radius_ * std::sin(angle);
    
    return Point_2(x, y);
}

double ArcEdge::distanceToPoint(const Point_2& point) const {
    double cx, cy, px, py;
    getPointCoords(center_, cx, cy);
    getPointCoords(point, px, py);
    
    double dx = px - cx;
    double dy = py - cy;
    double distToCenter = std::sqrt(dx * dx + dy * dy);
    double distToCircle = std::abs(distToCenter - radius_);
    
    // Check if closest point on circle is within arc range
    double angle = std::atan2(dy, dx);
    if (angle < 0) angle += 2 * M_PI;
    
    if (angleInRange(angle)) {
        return distToCircle;
    }
    
    // Return distance to closest endpoint
    double distToStart = std::sqrt(squaredDist(point, start_));
    double distToEnd = std::sqrt(squaredDist(point, end_));
    
    return std::min(distToStart, distToEnd);
}

bool ArcEdge::containsPoint(const Point_2& point, double tolerance) const {
    double cx, cy, px, py;
    getPointCoords(center_, cx, cy);
    getPointCoords(point, px, py);
    
    // Check if point is on the circle
    double dx = px - cx;
    double dy = py - cy;
    double distToCenter = std::sqrt(dx * dx + dy * dy);
    if (std::abs(distToCenter - radius_) > tolerance) {
        return false;
    }
    
    // Check if angle is within arc range
    double angle = std::atan2(dy, dx);
    if (angle < 0) angle += 2 * M_PI;
    
    return angleInRange(angle);
}

void ArcEdge::getAngleRange(double& startAngle, double& endAngle) const {
    double cx, cy, sx, sy, ex, ey;
    getPointCoords(center_, cx, cy);
    getPointCoords(start_, sx, sy);
    getPointCoords(end_, ex, ey);
    
    startAngle = std::atan2(sy - cy, sx - cx);
    endAngle = std::atan2(ey - cy, ex - cx);
    
    if (startAngle < 0) startAngle += 2 * M_PI;
    if (endAngle < 0) endAngle += 2 * M_PI;
}

bool ArcEdge::angleInRange(double angle) const {
    double startAngle, endAngle;
    getAngleRange(startAngle, endAngle);
    
    if (clockwise_) {
        // Clockwise: from startAngle down to endAngle
        if (startAngle >= endAngle) {
            return angle <= startAngle && angle >= endAngle;
        } else {
            return angle <= startAngle || angle >= endAngle;
        }
    } else {
        // Counter-clockwise: from startAngle up to endAngle
        if (endAngle >= startAngle) {
            return angle >= startAngle && angle <= endAngle;
        } else {
            return angle >= startAngle || angle <= endAngle;
        }
    }
}

// ==================== CurvedPolygon Implementation ====================

void CurvedPolygon::addLineSegment(const Point_2& start, const Point_2& end) {
    edges_.push_back(std::make_shared<LineEdge>(start, end));
}

void CurvedPolygon::addArc(const Point_2& start, const Point_2& end, const Point_2& mid) {
    edges_.push_back(std::make_shared<ArcEdge>(start, end, mid));
}

void CurvedPolygon::addArc(const Point_2& start, const Point_2& end, const Point_2& center, bool clockwise) {
    double cx, cy, sx, sy, ex, ey;
    getPointCoords(center, cx, cy);
    getPointCoords(start, sx, sy);
    getPointCoords(end, ex, ey);
    
    double radius = std::sqrt((sx - cx) * (sx - cx) + (sy - cy) * (sy - cy));
    double startAngle = std::atan2(sy - cy, sx - cx);
    double endAngle = std::atan2(ey - cy, ex - cx);
    
    if (startAngle < 0) startAngle += 2 * M_PI;
    if (endAngle < 0) endAngle += 2 * M_PI;
    
    double midAngle;
    if (clockwise) {
        if (startAngle > endAngle) {
            midAngle = (startAngle + endAngle) / 2;
        } else {
            midAngle = (startAngle + endAngle) / 2 + M_PI;
        }
    } else {
        if (endAngle > startAngle) {
            midAngle = (startAngle + endAngle) / 2;
        } else {
            midAngle = (startAngle + endAngle) / 2 + M_PI;
        }
    }
    
    if (midAngle >= 2 * M_PI) midAngle -= 2 * M_PI;
    
    Point_2 mid(cx + radius * std::cos(midAngle), cy + radius * std::sin(midAngle));
    
    edges_.push_back(std::make_shared<ArcEdge>(start, end, mid));
}

void CurvedPolygon::close() {
    if (edges_.size() > 1 && !closed_) {
        Point_2 firstPoint = edges_.front()->getStart();
        Point_2 lastPoint = edges_.back()->getEnd();
        
        if (squaredDist(firstPoint, lastPoint) > 1e-10) {
            addLineSegment(lastPoint, firstPoint);
        }
        closed_ = true;
    }
}

size_t CurvedPolygon::vertexCount() const {
    if (edges_.empty()) return 0;
    return closed_ ? edges_.size() : edges_.size() + 1;
}

Point_2 CurvedPolygon::getVertex(size_t index) const {
    if (index < edges_.size()) {
        return edges_[index]->getStart();
    } else if (index == edges_.size() && !closed_) {
        return edges_.back()->getEnd();
    }
    throw std::out_of_range("Vertex index out of range");
}

double CurvedPolygon::perimeter() const {
    double total = 0.0;
    for (const auto& edge : edges_) {
        total += edge->length();
    }
    return total;
}

double CurvedPolygon::area() const {
    // Approximate area using line segments
    double area = 0.0;
    
    for (const auto& edge : edges_) {
        if (edge->getType() == EdgeType::LINE_SEGMENT) {
            const LineEdge* line = static_cast<const LineEdge*>(edge.get());
            double x1, y1, x2, y2;
            getPointCoords(line->getStart(), x1, y1);
            getPointCoords(line->getEnd(), x2, y2);
            area += (x1 * y2 - x2 * y1) / 2.0;
        } else {
            // Approximate arc with line segments
            const ArcEdge* arc = static_cast<const ArcEdge*>(edge.get());
            int segments = 32;
            for (int i = 0; i < segments; ++i) {
                double t1 = static_cast<double>(i) / segments;
                double t2 = static_cast<double>(i + 1) / segments;
                Point_2 p1 = arc->pointAt(t1);
                Point_2 p2 = arc->pointAt(t2);
                double x1, y1, x2, y2;
                getPointCoords(p1, x1, y1);
                getPointCoords(p2, x2, y2);
                area += (x1 * y2 - x2 * y1) / 2.0;
            }
        }
    }
    
    return std::abs(area);
}

bool CurvedPolygon::containsPoint(const Point_2& point) const {
    if (!closed_ || edges_.empty()) {
        return false;
    }
    
    // Ray casting algorithm
    double px, py;
    getPointCoords(point, px, py);
    
    int intersectionCount = 0;
    
    for (const auto& edge : edges_) {
        if (edge->getType() == EdgeType::LINE_SEGMENT) {
            const LineEdge* line = static_cast<const LineEdge*>(edge.get());
            double x1, y1, x2, y2;
            getPointCoords(line->getStart(), x1, y1);
            getPointCoords(line->getEnd(), x2, y2);
            
            // Check if edge straddles horizontal line at py
            if ((y1 > py) != (y2 > py)) {
                double xIntersect = x1 + (py - y1) * (x2 - x1) / (y2 - y1);
                if (xIntersect > px) {
                    intersectionCount++;
                }
            }
        } else {
            // Approximate arc with line segments for ray casting
            const ArcEdge* arc = static_cast<const ArcEdge*>(edge.get());
            int segments = 32;
            for (int i = 0; i < segments; ++i) {
                double t1 = static_cast<double>(i) / segments;
                double t2 = static_cast<double>(i + 1) / segments;
                Point_2 p1 = arc->pointAt(t1);
                Point_2 p2 = arc->pointAt(t2);
                
                double x1, y1, x2, y2;
                getPointCoords(p1, x1, y1);
                getPointCoords(p2, x2, y2);
                
                if ((y1 > py) != (y2 > py)) {
                    double xIntersect = x1 + (py - y1) * (x2 - x1) / (y2 - y1);
                    if (xIntersect > px) {
                        intersectionCount++;
                    }
                }
            }
        }
    }
    
    return (intersectionCount % 2) == 1;
}

bool CurvedPolygon::pointOnBoundary(const Point_2& point, double tolerance) const {
    for (const auto& edge : edges_) {
        if (edge->containsPoint(point, tolerance)) {
            return true;
        }
    }
    return false;
}

int CurvedPolygon::pointRelation(const Point_2& point, double tolerance) const {
    if (pointOnBoundary(point, tolerance)) {
        return 0;  // On boundary
    }
    if (containsPoint(point)) {
        return 1;  // Inside
    }
    return -1;  // Outside
}

double CurvedPolygon::distanceToPoint(const Point_2& point) const {
    if (edges_.empty()) {
        return std::numeric_limits<double>::infinity();
    }
    
    double minDist = std::numeric_limits<double>::infinity();
    for (const auto& edge : edges_) {
        double dist = edge->distanceToPoint(point);
        if (dist < minDist) {
            minDist = dist;
        }
    }
    
    return minDist;
}

bool CurvedPolygon::isSimple() const {
    // Simplified: assume polygon is simple
    return true;
}

void CurvedPolygon::getBoundingBox(double& minX, double& maxX, double& minY, double& maxY) const {
    if (edges_.empty()) {
        minX = maxX = minY = maxY = 0.0;
        return;
    }
    
    minX = minY = std::numeric_limits<double>::infinity();
    maxX = maxY = -std::numeric_limits<double>::infinity();
    
    for (const auto& edge : edges_) {
        if (edge->getType() == EdgeType::LINE_SEGMENT) {
            const LineEdge* line = static_cast<const LineEdge*>(edge.get());
            double x1, y1, x2, y2;
            getPointCoords(line->getStart(), x1, y1);
            getPointCoords(line->getEnd(), x2, y2);
            
            minX = std::min(minX, std::min(x1, x2));
            maxX = std::max(maxX, std::max(x1, x2));
            minY = std::min(minY, std::min(y1, y2));
            maxY = std::max(maxY, std::max(y1, y2));
        } else {
            const ArcEdge* arc = static_cast<const ArcEdge*>(edge.get());
            // Sample arc points for bounding box
            int samples = 16;
            for (int i = 0; i <= samples; ++i) {
                double t = static_cast<double>(i) / samples;
                Point_2 p = arc->pointAt(t);
                double x, y;
                getPointCoords(p, x, y);
                minX = std::min(minX, x);
                maxX = std::max(maxX, x);
                minY = std::min(minY, y);
                maxY = std::max(maxY, y);
            }
        }
    }
}

void CurvedPolygon::clear() {
    edges_.clear();
    closed_ = false;
}

Polygon_2 CurvedPolygon::toPolygon(int arcSegments) const {
    Polygon_2 polygon;
    
    for (const auto& edge : edges_) {
        if (edge->getType() == EdgeType::LINE_SEGMENT) {
            polygon.push_back(edge->getStart());
        } else {
            // Approximate arc with line segments
            const ArcEdge* arc = static_cast<const ArcEdge*>(edge.get());
            for (int i = 0; i < arcSegments; ++i) {
                double t = static_cast<double>(i) / arcSegments;
                polygon.push_back(arc->pointAt(t));
            }
        }
    }
    
    return polygon;
}
