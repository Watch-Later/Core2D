﻿using System.Collections.Generic;
using Core2D.Renderer;

namespace Core2D.Shapes
{
    public static class GroupShapeExtensions
    {
        public static void AddShape(this GroupShapeViewModel group, BaseShapeViewModel shapeViewModel)
        {
            shapeViewModel.Owner = group;
            shapeViewModel.State &= ~ShapeStateFlags.Standalone;
            group.Shapes = group.Shapes.Add(shapeViewModel);
        }

        public static void Group(this GroupShapeViewModel group, IEnumerable<BaseShapeViewModel> shapes, IList<BaseShapeViewModel> source = null)
        {
            if (shapes != null)
            {
                foreach (var shape in shapes)
                {
                    if (shape is PointShapeViewModel)
                    {
                        group.AddConnectorAsNone(shape as PointShapeViewModel);
                    }
                    else
                    {
                        group.AddShape(shape);
                    }

                    source?.Remove(shape);
                }
            }

            source?.Add(@group);
        }

        public static void Ungroup(IEnumerable<BaseShapeViewModel> shapes, IList<BaseShapeViewModel> source)
        {
            if (shapes != null && source != null)
            {
                foreach (var shape in shapes)
                {
                    if (shape is PointShapeViewModel point)
                    {
                        point.State &=
                            ~(ShapeStateFlags.Connector
                            | ShapeStateFlags.None
                            | ShapeStateFlags.Input
                            | ShapeStateFlags.Output);
                    }

                    shape.State |= ShapeStateFlags.Standalone;

                    source?.Add(shape);
                }
            }
        }

        public static void Ungroup(this GroupShapeViewModel group, IList<BaseShapeViewModel> source)
        {
            Ungroup(group.Shapes, source);
            Ungroup(group.Connectors, source);

            source?.Remove(@group);
        }
    }
}
