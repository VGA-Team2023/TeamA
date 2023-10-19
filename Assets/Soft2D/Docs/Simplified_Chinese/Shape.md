# Shape
Shape 用于描述一个 2D 的形状。主要用于 Soft2D 物体（collider、trigger 和 body）的创建。

## Box（长方形）
- 半宽度
  - 长方形宽度的一半。
- 半高度
  - 长方形高度的一半。

## Circle（圆形）
- 半径
  - 圆的半径。

## Ellipse（椭圆形）
- 水平半径
  - 椭圆在水平方向上的半径。
- 垂直半径
  - 椭圆在垂直方向上的半径。

## Capsule（胶囊体形）
- 中心长方形半边长
  - 胶囊体中心长方形的边长的一半。
- 顶端半径
  - 胶囊体顶端（半）圆形的半径。

## Polygon（多边形）
- 顶点坐标
  - 多边形所有端点**沿逆时针**排序在（以多边形中心为原点的）局部空间中的位置坐标。