#include "HoleCreationProcessor.h"
#include "GeometryUtils.h"
#include "PolygonValidator.h"
#include <iostream>

int main() {
    std::cout << "CGAL Hole Creation - With Self-Intersection Repair" << std::endl;
    std::cout << "===================================================" << std::endl;

    // Create processor
    HoleCreationProcessor processor(800, 600);

    // ========================================
    // Test 1: Normal polygon (no self-intersection)
    // ========================================
    std::cout << "\n[Test 1] Normal rectangle with holes" << std::endl;
    Polygon_2 outer_rect = createRectangle(0, 0, 10, 8);
    PolygonValidator::printPolygonInfo(outer_rect, "Outer Rectangle");
    
    auto outer_rect_geom = std::make_shared<PolygonGeometry>(outer_rect, "lightblue", "blue", 2);
    PolygonWithHoles rect_with_holes(outer_rect_geom);

    // Add rectangular holes
    Polygon_2 hole_rect1 = createRectangle(2, 2, 4, 4);
    rect_with_holes.addHole(std::make_shared<PolygonGeometry>(hole_rect1, "white", "blue", 2));

    Polygon_2 hole_rect2 = createRectangle(6, 5, 8, 7);
    rect_with_holes.addHole(std::make_shared<PolygonGeometry>(hole_rect2, "white", "blue", 2));

    // Add circular holes
    auto circle1 = std::make_shared<CircleGeometry>(Point_2(5, 2), 1.5, 32);
    auto circle2 = std::make_shared<CircleGeometry>(Point_2(3, 6), 1.0, 32);
    rect_with_holes.addCircleHole(circle1);
    rect_with_holes.addCircleHole(circle2);

    processor.addPolygonWithHoles(rect_with_holes);

    // ========================================
    // Test 2: Triangle with hole
    // ========================================
    std::cout << "\n[Test 2] Triangle with circular hole" << std::endl;
    Polygon_2 outer_triangle = createTriangle(12, 0, 17, 0, 14.5, 5);
    PolygonValidator::printPolygonInfo(outer_triangle, "Outer Triangle");
    
    auto outer_triangle_geom = std::make_shared<PolygonGeometry>(outer_triangle, "lightgreen", "green", 2);
    PolygonWithHoles triangle_with_holes(outer_triangle_geom);

    auto circle3 = std::make_shared<CircleGeometry>(Point_2(14.5, 2), 1.2, 32);
    triangle_with_holes.addCircleHole(circle3);

    processor.addPolygonWithHoles(triangle_with_holes);

    // ========================================
    // Test 3: Self-intersecting polygon (8-shape)
    // ========================================
    std::cout << "\n[Test 3] Self-intersecting polygon (8-shape) - Will be repaired" << std::endl;
    Polygon_2 self_intersecting;
    self_intersecting.push_back(Point_2(20, 0));   // 0
    self_intersecting.push_back(Point_2(24, 4));   // 1 - crosses with edge 3-4
    self_intersecting.push_back(Point_2(24, 0));   // 2
    self_intersecting.push_back(Point_2(20, 4));   // 3 - crosses with edge 0-1
    
    PolygonValidator::printPolygonInfo(self_intersecting, "Self-Intersecting Polygon");
    
    // Repair the self-intersecting polygon
    Polygon_with_holes_2 repaired = PolygonValidator::autoRepair(self_intersecting);
    std::cout << "  Repaired polygon has " << repaired.number_of_holes() << " holes" << std::endl;
    
    // Create PolygonWithHoles from repaired polygon
    auto repaired_geom = std::make_shared<PolygonGeometry>(repaired.outer_boundary(), "lightyellow", "orange", 2);
    PolygonWithHoles repaired_poly(repaired_geom);
    
    // Add any holes from repair
    for (auto hit = repaired.holes_begin(); hit != repaired.holes_end(); ++hit) {
        repaired_poly.addHole(std::make_shared<PolygonGeometry>(*hit, "white", "orange", 2));
    }
    
    processor.addPolygonWithHoles(repaired_poly);

    // ========================================
    // Test 4: Complex self-intersecting polygon (Star shape)
    // ========================================
    std::cout << "\n[Test 4] Complex self-intersecting polygon (Star) - Will be repaired" << std::endl;
    Polygon_2 star_poly;
    // Create a self-intersecting star
    star_poly.push_back(Point_2(28, 0));   // 0 - bottom
    star_poly.push_back(Point_2(30, 3));   // 1 - inner
    star_poly.push_back(Point_2(32, 0));   // 2 - bottom-right
    star_poly.push_back(Point_2(31, 4));   // 3 - right
    star_poly.push_back(Point_2(34, 5));   // 4 - top-right
    star_poly.push_back(Point_2(31, 6));   // 5 - top
    star_poly.push_back(Point_2(30, 8));   // 6 - top (crosses with other edges)
    star_poly.push_back(Point_2(29, 6));   // 7 - top-left
    star_poly.push_back(Point_2(26, 5));   // 8 - left
    star_poly.push_back(Point_2(29, 4));   // 9 - left (crosses)
    
    PolygonValidator::printPolygonInfo(star_poly, "Star Polygon");
    
    // Repair
    Polygon_with_holes_2 repaired_star = PolygonValidator::autoRepair(star_poly);
    std::cout << "  Repaired star has " << repaired_star.number_of_holes() << " holes" << std::endl;
    
    auto star_geom = std::make_shared<PolygonGeometry>(repaired_star.outer_boundary(), "lightpink", "purple", 2);
    PolygonWithHoles star_with_holes(star_geom);
    
    for (auto hit = repaired_star.holes_begin(); hit != repaired_star.holes_end(); ++hit) {
        star_with_holes.addHole(std::make_shared<PolygonGeometry>(*hit, "white", "purple", 2));
    }
    
    processor.addPolygonWithHoles(star_with_holes);

    // Add reference circles
    processor.addReferenceCircle(circle1);
    processor.addReferenceCircle(circle2);
    processor.addReferenceCircle(circle3);

    // Print statistics
    processor.printStatistics();

    // Save to SVG
    processor.saveToSVG("holes_with_repair.svg");
    std::cout << "\nResult saved to: holes_with_repair.svg" << std::endl;
    std::cout << "Open holes_with_repair.svg in your web browser to view the result." << std::endl;

    return 0;
}
