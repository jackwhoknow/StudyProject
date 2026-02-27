#include "Geometry.h"
#include <iostream>

int main() {
    std::cout << "CGAL Hole Creation - Class-based Implementation" << std::endl;
    std::cout << "================================================" << std::endl;

    // Create processor
    HoleCreationProcessor processor(800, 600);

    // Create first polygon with holes (large rectangle)
    Polygon_2 outer_rect = createRectangle(0, 0, 10, 8);
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

    // Create second polygon with holes (triangle)
    Polygon_2 outer_triangle = createTriangle(12, 0, 17, 0, 14.5, 5);
    auto outer_triangle_geom = std::make_shared<PolygonGeometry>(outer_triangle, "lightgreen", "green", 2);
    PolygonWithHoles triangle_with_holes(outer_triangle_geom);

    // Add circular hole
    auto circle3 = std::make_shared<CircleGeometry>(Point_2(14.5, 2), 1.2, 32);
    triangle_with_holes.addCircleHole(circle3);

    processor.addPolygonWithHoles(triangle_with_holes);

    // Add reference circles (for display only)
    processor.addReferenceCircle(circle1);
    processor.addReferenceCircle(circle2);
    processor.addReferenceCircle(circle3);

    // Print statistics
    processor.printStatistics();

    // Save to SVG
    processor.saveToSVG("holes_class.svg");
    std::cout << "\nResult saved to: holes_class.svg" << std::endl;
    std::cout << "Open holes_class.svg in your web browser to view the result." << std::endl;

    return 0;
}
