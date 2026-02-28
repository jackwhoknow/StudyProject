#include "PolygonValidator.h"
#include <iostream>
#include <CGAL/Polygon_repair/repair.h>
#include <CGAL/Boolean_set_operations_2.h>
#include <CGAL/Polygon_2_algorithms.h>

// Check if polygon has self-intersection
bool PolygonValidator::hasSelfIntersection(const Polygon_2& poly) {
    if (poly.size() < 3) return false;
    
    // Use CGAL is_simple_2 to check if polygon is simple (no self-intersection)
    return !CGAL::is_simple_2(poly.vertices_begin(), poly.vertices_end());
}

// Check if polygon is valid
bool PolygonValidator::isValid(const Polygon_2& poly) {
    // Check vertex count
    if (poly.size() < 3) {
        return false;
    }
    
    // Check if simple (no self-intersection)
    if (!CGAL::is_simple_2(poly.vertices_begin(), poly.vertices_end())) {
        return false;
    }
    
    return true;
}

// Get self-intersection count
int PolygonValidator::getSelfIntersectionCount(const Polygon_2& poly) {
    if (poly.size() < 4) return 0;
    
    int count = 0;
    int n = poly.size();
    
    // Check each pair of edges for intersection
    for (int i = 0; i < n; ++i) {
        Point_2 p1 = poly[i];
        Point_2 p2 = poly[(i + 1) % n];
        
        for (int j = i + 2; j < n; ++j) {
            // Skip adjacent edges
            if (j == (i + n - 1) % n) continue;
            
            Point_2 p3 = poly[j];
            Point_2 p4 = poly[(j + 1) % n];
            
            // Check if two segments intersect
            if (CGAL::do_intersect(
                K::Segment_2(p1, p2), 
                K::Segment_2(p3, p4))) {
                count++;
            }
        }
    }
    
    return count;
}

// Print polygon status information
void PolygonValidator::printPolygonInfo(const Polygon_2& poly, const std::string& name) {
    std::cout << "\n" << name << " Information:" << std::endl;
    std::cout << "  Vertices: " << poly.size() << std::endl;
    std::cout << "  Area: " << computeArea(poly) << std::endl;
    bool simple = isSimple(poly);
    std::cout << "  Is simple (no self-intersection): " << (simple ? "Yes" : "No") << std::endl;
    if (simple) {
        std::cout << "  Is convex: " << (isConvex(poly) ? "Yes" : "No") << std::endl;
        std::cout << "  Orientation: " << (isCounterClockwise(poly) ? "Counter-clockwise" : "Clockwise") << std::endl;
    } else {
        std::cout << "  Is convex: N/A (not simple)" << std::endl;
        std::cout << "  Orientation: N/A (not simple)" << std::endl;
        std::cout << "  Self-intersections: " << getSelfIntersectionCount(poly) << std::endl;
    }
}

// Use even-odd rule to repair self-intersecting polygon
Multipolygon_with_holes_2 PolygonValidator::repairEvenOdd(const Polygon_2& poly) {
    // Use CGAL Polygon_repair with even-odd rule
    return CGAL::Polygon_repair::repair(poly, CGAL::Polygon_repair::Even_odd_rule());
}

// Use non-zero rule to repair self-intersecting polygon
Multipolygon_with_holes_2 PolygonValidator::repairNonZero(const Polygon_2& poly) {
    // Use CGAL Polygon_repair with non-zero rule
    return CGAL::Polygon_repair::repair(poly, CGAL::Polygon_repair::Non_zero_rule());
}

// Auto-detect issues and repair self-intersection
Polygon_with_holes_2 PolygonValidator::autoRepair(const Polygon_2& poly) {
    if (CGAL::is_simple_2(poly.vertices_begin(), poly.vertices_end())) {
        // No self-intersection, return directly
        return Polygon_with_holes_2(poly);
    }
    
    std::cout << "  Detected self-intersection, repairing..." << std::endl;
    
    // Repair using even-odd rule
    Multipolygon_with_holes_2 repaired = repairEvenOdd(poly);
    
    // Return first polygon from result, or empty polygon if none
    if (repaired.number_of_polygons_with_holes() == 0) {
        return Polygon_with_holes_2();
    }
    
    // Return the first polygon
    auto it = repaired.polygons_with_holes_begin();
    return *it;
}

// Check if polygon is simple
bool PolygonValidator::isSimple(const Polygon_2& poly) {
    return CGAL::is_simple_2(poly.vertices_begin(), poly.vertices_end());
}

// Check if polygon is convex
bool PolygonValidator::isConvex(const Polygon_2& poly) {
    return CGAL::is_convex_2(poly.vertices_begin(), poly.vertices_end());
}

// Compute polygon area
double PolygonValidator::computeArea(const Polygon_2& poly) {
    return CGAL::to_double(poly.area());
}

// Check polygon orientation
bool PolygonValidator::isCounterClockwise(const Polygon_2& poly) {
    return poly.orientation() == CGAL::COUNTERCLOCKWISE;
}

// Force polygon to counter-clockwise direction
void PolygonValidator::makeCounterClockwise(Polygon_2& poly) {
    if (poly.orientation() == CGAL::CLOCKWISE) {
        // Reverse vertex order
        Polygon_2 reversed;
        for (auto it = poly.vertices_end(); it != poly.vertices_begin(); ) {
            --it;
            reversed.push_back(*it);
        }
        poly = reversed;
    }
}
