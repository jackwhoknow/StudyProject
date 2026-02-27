#include <CGAL/Exact_predicates_inexact_constructions_kernel.h>
#include <CGAL/Triangulation_2.h>
#include <vector>
#include <iostream>
#include <fstream>
#include <cmath>
#include <string>

typedef CGAL::Exact_predicates_inexact_constructions_kernel K;
typedef CGAL::Triangulation_2<K> Triangulation;
typedef Triangulation::Point Point;

// 绘制虚线的方法
void drawDashedLineToSVG(std::ofstream& svg, double x1, double y1, double x2, double y2, 
                         std::string color, int strokeWidth,
                         std::string dashArray) {
    svg << "  <line x1=\"" << x1 << "\" y1=\"" << y1 << "\" x2=\"" << x2 << "\" y2=\"" << y2 << "\" ";
    svg << "stroke=\"" << color << "\" stroke-width=\"" << strokeWidth << "\" ";
    svg << "stroke-dasharray=\"" << dashArray << "\"/>\n";
}

void drawTriangleToSVG(const Triangulation& t, const std::string& filename) {
    std::ofstream svg(filename);
    
    // SVG header
    svg << "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
    svg << "<svg width=\"500\" height=\"500\" xmlns=\"http://www.w3.org/2000/svg\">\n";
    svg << "  <rect width=\"500\" height=\"500\" fill=\"white\"/>\n";
    
    // Calculate bounding box
    double min_x = 1e10, max_x = -1e10, min_y = 1e10, max_y = -1e10;
    for (auto vit = t.finite_vertices_begin(); vit != t.finite_vertices_end(); ++vit) {
        double x = CGAL::to_double(vit->point().x());
        double y = CGAL::to_double(vit->point().y());
        min_x = std::min(min_x, x);
        max_x = std::max(max_x, x);
        min_y = std::min(min_y, y);
        max_y = std::max(max_y, y);
    }
    
    // Add some padding
    double padding = 50;
    double width = max_x - min_x;
    double height = max_y - min_y;
    double scale = std::min((500 - 2 * padding) / width, (500 - 2 * padding) / height);
    
    double offset_x = (500 - width * scale) / 2 - min_x * scale;
    double offset_y = (500 - height * scale) / 2 - min_y * scale;
    
    // Transform function
    auto transform = [&](double x, double y) -> std::pair<double, double> {
        return {x * scale + offset_x, 500 - (y * scale + offset_y)}; // Flip Y axis
    };
    
    // Draw triangle edges
    svg << "  <!-- Triangle edges -->\n";
    for (auto fit = t.finite_faces_begin(); fit != t.finite_faces_end(); ++fit) {
        for (int i = 0; i < 3; ++i) {
            auto p1 = fit->vertex((i + 1) % 3)->point();
            auto p2 = fit->vertex((i + 2) % 3)->point();
            
            auto [x1, y1] = transform(CGAL::to_double(p1.x()), CGAL::to_double(p1.y()));
            auto [x2, y2] = transform(CGAL::to_double(p2.x()), CGAL::to_double(p2.y()));
            
            svg << "  <line x1=\"" << x1 << "\" y1=\"" << y1 << "\" x2=\"" << x2 << "\" y2=\"" << y2 << "\" ";
            svg << "stroke=\"blue\" stroke-width=\"3\"/>\n";
        }
    }
    
    // Draw vertices
    svg << "  <!-- Vertices -->\n";
    for (auto vit = t.finite_vertices_begin(); vit != t.finite_vertices_end(); ++vit) {
        double x = CGAL::to_double(vit->point().x());
        double y = CGAL::to_double(vit->point().y());
        auto [px, py] = transform(x, y);
        
        svg << "  <circle cx=\"" << px << "\" cy=\"" << py << "\" r=\"8\" fill=\"red\"/>\n";
        svg << "  <text x=\"" << (px + 12) << "\" y=\"" << (py - 12) << "\" font-size=\"14\" fill=\"black\"\u003e";
        svg << "(" << x << ", " << y << ")\u003c/text\u003e\n";
    }
    
    // Title
    svg << "  <text x=\"250\" y=\"30\" text-anchor=\"middle\" font-size=\"20\" fill=\"black\">CGAL Triangle</text>\n";
    
    // Draw red dashed lines from centroid to vertices
    svg << "  <!-- Red dashed lines from centroid to vertices -->\n";
    double centroid_x = 0, centroid_y = 0;
    int vertex_count = 0;
    for (auto vit = t.finite_vertices_begin(); vit != t.finite_vertices_end(); ++vit) {
        centroid_x += CGAL::to_double(vit->point().x());
        centroid_y += CGAL::to_double(vit->point().y());
        vertex_count++;
    }
    if (vertex_count > 0) {
        centroid_x /= vertex_count;
        centroid_y /= vertex_count;
        auto [cx, cy] = transform(centroid_x, centroid_y);
        
        // Draw red dashed lines from centroid to each vertex
        for (auto vit = t.finite_vertices_begin(); vit != t.finite_vertices_end(); ++vit) {
            double vx = CGAL::to_double(vit->point().x());
            double vy = CGAL::to_double(vit->point().y());
            auto [px, py] = transform(vx, vy);
            drawDashedLineToSVG(svg, cx, cy, px, py, "red", 2, "10,5");
        }
    }
    
    svg << "</svg>\n";
    svg.close();
}

int main()
{
    std::vector<Point> points;
    points.push_back(Point(0, 0));
    points.push_back(Point(1, 0));
    points.push_back(Point(0.5, 1));

    Triangulation t;
    t.insert(points.begin(), points.end());

    std::cout << "Triangle vertices:" << std::endl;
    for (auto vit = t.finite_vertices_begin(); vit != t.finite_vertices_end(); ++vit)
    {
        std::cout << "(" << vit->point().x() << ", " << vit->point().y() << ")" << std::endl;
    }

    std::cout << "\nNumber of finite faces: " << t.number_of_faces() << std::endl;
    
    // Generate SVG file
    drawTriangleToSVG(t, "triangle.svg");
    std::cout << "\nTriangle visualization saved to: triangle.svg" << std::endl;
    std::cout << "Open triangle.svg in your web browser to view the triangle." << std::endl;

    return 0;
}
