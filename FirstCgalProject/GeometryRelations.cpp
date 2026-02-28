#include "GeometryRelations.h"
#include <iostream>
#include <cmath>

// ========== ???????¦É?? ==========

PointPolygonRelation GeometryRelations::pointInPolygon(const Point_2& point, const Polygon_2& poly) {
    // ???????????????¦Á???
    if (isPointOnPolygonEdge(point, poly)) {
        return PointPolygonRelation::ON_EDGE;
    }
    
    // ???CGAL??bounded_side?????§Ø???????¦Å?¦Ë?¨´??
    CGAL::Bounded_side side = poly.bounded_side(point);
    
    if (side == CGAL::ON_BOUNDED_SIDE) {
        return PointPolygonRelation::INSIDE;
    } else {
        return PointPolygonRelation::OUTSIDE;
    }
}

bool GeometryRelations::isPointInsidePolygon(const Point_2& point, const Polygon_2& poly) {
    return pointInPolygon(point, poly) == PointPolygonRelation::INSIDE;
}

bool GeometryRelations::isPointOnPolygonEdge(const Point_2& point, const Polygon_2& poly, double tolerance) {
    // ????????¦Å????§Ò?
    for (auto edge_it = poly.edges_begin(); edge_it != poly.edges_end(); ++edge_it) {
        // ???CGAL??has_on??????????????????
        if (edge_it->has_on(point)) {
            return true;
        }
        
        // ????????????????????¦Å????
        if (tolerance > 0) {
            double dist = distancePointToSegment(point, edge_it->source(), edge_it->target());
            if (dist <= tolerance) {
                return true;
            }
        }
    }
    return false;
}

bool GeometryRelations::isPointOutsidePolygon(const Point_2& point, const Polygon_2& poly) {
    return pointInPolygon(point, poly) == PointPolygonRelation::OUTSIDE;
}

double GeometryRelations::signedDistanceToPolygon(const Point_2& point, const Polygon_2& poly) {
    PointPolygonRelation relation = pointInPolygon(point, poly);
    
    if (relation == PointPolygonRelation::ON_EDGE) {
        return 0.0;
    }
    
    double minDist = distanceToPolygonEdge(point, poly);
    
    if (relation == PointPolygonRelation::INSIDE) {
        return -minDist;  // ??????
    } else {
        return minDist;   // ?????
    }
}

double GeometryRelations::distanceToPolygonEdge(const Point_2& point, const Polygon_2& poly) {
    double minDist = std::numeric_limits<double>::max();
    
    for (auto edge_it = poly.edges_begin(); edge_it != poly.edges_end(); ++edge_it) {
        double dist = distancePointToSegment(point, edge_it->source(), edge_it->target());
        if (dist < minDist) {
            minDist = dist;
        }
    }
    
    return minDist;
}

// ========== ???????/??¦É?? ==========

PointLineRelation GeometryRelations::pointToLineRelation(const Point_2& point, 
                                                          const Point_2& lineStart, 
                                                          const Point_2& lineEnd) {
    // ?????????????????
    if (isPointOnSegment(point, lineStart, lineEnd)) {
        if (isPointOnSegmentInterior(point, lineStart, lineEnd)) {
            return PointLineRelation::ON_SEGMENT_INTERIOR;
        } else {
            return PointLineRelation::ON_SEGMENT;
        }
    }
    
    // ???????????????????????????
    if (isPointOnLine(point, lineStart, lineEnd)) {
        return PointLineRelation::ON_LINE;
    }
    
    // ?§Ø?????????????
    CGAL::Oriented_side side = pointSideOfLine(point, lineStart, lineEnd);
    
    if (side == CGAL::ON_POSITIVE_SIDE) {
        return PointLineRelation::LEFT_OF_LINE;
    } else if (side == CGAL::ON_NEGATIVE_SIDE) {
        return PointLineRelation::RIGHT_OF_LINE;
    } else {
        // ?????????????????????????????????
        return PointLineRelation::ON_LINE;
    }
}

bool GeometryRelations::isPointOnLine(const Point_2& point, 
                                     const Point_2& lineStart, 
                                     const Point_2& lineEnd, 
                                     double tolerance) {
    // ???CGAL???????
    if (CGAL::collinear(lineStart, lineEnd, point)) {
        return true;
    }
    
    // ?????????????????????????
    if (tolerance > 0) {
        double dist = distancePointToLine(point, lineStart, lineEnd);
        return dist <= tolerance;
    }
    
    return false;
}

bool GeometryRelations::isPointOnSegment(const Point_2& point, 
                                        const Point_2& segStart, 
                                        const Point_2& segEnd, 
                                        double tolerance) {
    // ?????????????????
    if (!isPointOnLine(point, segStart, segEnd, tolerance)) {
        return false;
    }
    
    // ???????????¦Å??¦¶????
    double minX = std::min(CGAL::to_double(segStart.x()), CGAL::to_double(segEnd.x())) - tolerance;
    double maxX = std::max(CGAL::to_double(segStart.x()), CGAL::to_double(segEnd.x())) + tolerance;
    double minY = std::min(CGAL::to_double(segStart.y()), CGAL::to_double(segEnd.y())) - tolerance;
    double maxY = std::max(CGAL::to_double(segStart.y()), CGAL::to_double(segEnd.y())) + tolerance;
    
    double px = CGAL::to_double(point.x());
    double py = CGAL::to_double(point.y());
    
    return (px >= minX && px <= maxX && py >= minY && py <= maxY);
}

bool GeometryRelations::isPointOnSegmentInterior(const Point_2& point, 
                                                const Point_2& segStart, 
                                                const Point_2& segEnd, 
                                                double tolerance) {
    // ?????????????????
    if (!isPointOnSegment(point, segStart, segEnd, tolerance)) {
        return false;
    }
    
    // ???????????
    double distToStart = CGAL::to_double(CGAL::squared_distance(point, segStart));
    double distToEnd = CGAL::to_double(CGAL::squared_distance(point, segEnd));
    
    return (distToStart > tolerance * tolerance && distToEnd > tolerance * tolerance);
}

CGAL::Oriented_side GeometryRelations::pointSideOfLine(const Point_2& point, 
                                                       const Point_2& lineStart, 
                                                       const Point_2& lineEnd) {
    // ???CGAL??orientation????
    CGAL::Orientation orient = CGAL::orientation(lineStart, lineEnd, point);
    
    if (orient == CGAL::LEFT_TURN) {
        return CGAL::ON_POSITIVE_SIDE;  // ???
    } else if (orient == CGAL::RIGHT_TURN) {
        return CGAL::ON_NEGATIVE_SIDE;  // ???
    } else {
        return CGAL::ON_ORIENTED_BOUNDARY;  // ??????
    }
}

double GeometryRelations::distancePointToLine(const Point_2& point, 
                                             const Point_2& lineStart, 
                                             const Point_2& lineEnd) {
    // ???CGAL????????????
    K::Line_2 line(lineStart, lineEnd);
    return CGAL::to_double(CGAL::sqrt(CGAL::squared_distance(point, line)));
}

double GeometryRelations::distancePointToSegment(const Point_2& point, 
                                                const Point_2& segStart, 
                                                const Point_2& segEnd) {
    // ???CGAL???????¦Å????
    K::Segment_2 seg(segStart, segEnd);
    return CGAL::to_double(CGAL::sqrt(CGAL::squared_distance(point, seg)));
}

Point_2 GeometryRelations::projectPointToSegment(const Point_2& point, 
                                                const Point_2& segStart, 
                                                const Point_2& segEnd) {
    // ????????
    K::Segment_2 seg(segStart, segEnd);
    K::Line_2 line(segStart, segEnd);
    
    // ???????
    K::Point_2 proj = line.projection(point);
    
    // ??????????????¦Ç?¦¶??
    if (isPointOnSegment(proj, segStart, segEnd)) {
        return proj;
    }
    
    // ????????¦¶??????????????
    double distToStart = CGAL::to_double(CGAL::squared_distance(point, segStart));
    double distToEnd = CGAL::to_double(CGAL::squared_distance(point, segEnd));
    
    return (distToStart < distToEnd) ? segStart : segEnd;
}

// ========== ???????? ==========

const char* GeometryRelations::pointPolygonRelationToString(PointPolygonRelation relation) {
    switch (relation) {
        case PointPolygonRelation::INSIDE:  return "INSIDE";
        case PointPolygonRelation::ON_EDGE: return "ON_EDGE";
        case PointPolygonRelation::OUTSIDE: return "OUTSIDE";
        default: return "UNKNOWN";
    }
}

const char* GeometryRelations::pointLineRelationToString(PointLineRelation relation) {
    switch (relation) {
        case PointLineRelation::ON_LINE:            return "ON_LINE";
        case PointLineRelation::ON_SEGMENT:         return "ON_SEGMENT";
        case PointLineRelation::ON_SEGMENT_INTERIOR: return "ON_SEGMENT_INTERIOR";
        case PointLineRelation::LEFT_OF_LINE:       return "LEFT_OF_LINE";
        case PointLineRelation::RIGHT_OF_LINE:      return "RIGHT_OF_LINE";
        case PointLineRelation::OUTSIDE_SEGMENT:    return "OUTSIDE_SEGMENT";
        default: return "UNKNOWN";
    }
}

void GeometryRelations::printPointPolygonRelation(const Point_2& point, const Polygon_2& poly) {
    std::cout << "\nPoint-Polygon Relation:" << std::endl;
    std::cout << "  Point: (" << CGAL::to_double(point.x()) << ", " << CGAL::to_double(point.y()) << ")" << std::endl;
    std::cout << "  Polygon vertices: " << poly.size() << std::endl;
    
    PointPolygonRelation relation = pointInPolygon(point, poly);
    std::cout << "  Relation: " << pointPolygonRelationToString(relation) << std::endl;
    
    double dist = distanceToPolygonEdge(point, poly);
    std::cout << "  Distance to edge: " << dist << std::endl;
}

void GeometryRelations::printPointSegmentRelation(const Point_2& point, 
                                                 const Point_2& segStart, 
                                                 const Point_2& segEnd) {
    std::cout << "\nPoint-Segment Relation:" << std::endl;
    std::cout << "  Point: (" << CGAL::to_double(point.x()) << ", " << CGAL::to_double(point.y()) << ")" << std::endl;
    std::cout << "  Segment: (" << CGAL::to_double(segStart.x()) << ", " << CGAL::to_double(segStart.y()) << ") - ("
              << CGAL::to_double(segEnd.x()) << ", " << CGAL::to_double(segEnd.y()) << ")" << std::endl;
    
    PointLineRelation relation = pointToLineRelation(point, segStart, segEnd);
    std::cout << "  Relation: " << pointLineRelationToString(relation) << std::endl;
    
    double dist = distancePointToSegment(point, segStart, segEnd);
    std::cout << "  Distance to segment: " << dist << std::endl;
}
