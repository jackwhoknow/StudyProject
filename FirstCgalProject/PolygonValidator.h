#ifndef POLYGON_VALIDATOR_H
#define POLYGON_VALIDATOR_H

#include "GeometryBase.h"
#include <CGAL/Polygon_2_algorithms.h>
#include <CGAL/Polygon_with_holes_2.h>
#include <CGAL/Multipolygon_with_holes_2.h>
#include <vector>

// Forward declarations
typedef CGAL::Polygon_with_holes_2<K> Polygon_with_holes_2;
typedef CGAL::Multipolygon_with_holes_2<K, std::vector<Point_2>> Multipolygon_with_holes_2;

// Polygon Validator and Repair class
class PolygonValidator {
public:
    // Check if polygon has self-intersection
    static bool hasSelfIntersection(const Polygon_2& poly);
    
    // Check if polygon is valid (no self-intersection, no self-touch, vertices >= 3)
    static bool isValid(const Polygon_2& poly);
    
    // Get self-intersection count
    static int getSelfIntersectionCount(const Polygon_2& poly);
    
    // Print polygon status information
    static void printPolygonInfo(const Polygon_2& poly, const std::string& name = "Polygon");
    
    // Use even-odd rule to repair self-intersecting polygon
    // Returns repaired multipolygon (may contain multiple polygons)
    static Multipolygon_with_holes_2 repairEvenOdd(const Polygon_2& poly);
    
    // Use non-zero rule to repair self-intersecting polygon
    static Multipolygon_with_holes_2 repairNonZero(const Polygon_2& poly);
    
    // Auto-detect issues and repair self-intersection
    // Returns first polygon from repaired result, or original polygon if no self-intersection
    static Polygon_with_holes_2 autoRepair(const Polygon_2& poly);
    
    // Check if polygon is simple (no self-intersection)
    static bool isSimple(const Polygon_2& poly);
    
    // Check if polygon is convex
    static bool isConvex(const Polygon_2& poly);
    
    // Compute polygon area (absolute value)
    static double computeArea(const Polygon_2& poly);
    
    // Check polygon orientation (counter-clockwise/clockwise)
    static bool isCounterClockwise(const Polygon_2& poly);
    
    // Force polygon to counter-clockwise direction
    static void makeCounterClockwise(Polygon_2& poly);
};

#endif // POLYGON_VALIDATOR_H
