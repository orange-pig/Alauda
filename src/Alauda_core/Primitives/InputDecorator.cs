using Alauda.Enumerator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Alauda.Standard;
using System.Collections.ObjectModel;

namespace Alauda
{
    public class InputDecorator : Decorator
    {
        public static readonly DependencyProperty BackgroundProperty =
                Panel.BackgroundProperty.AddOwner(typeof(InputDecorator),
                        new FrameworkPropertyMetadata(
                                (Brush)null,
                                FrameworkPropertyMetadataOptions.AffectsRender));


        UIElement _prefix = null;
        UIElement _suffix = null;

        public InputDecorator() : base()
        {
        }


        /// <summary>
        /// The Background property defines the brush used to fill the area within the InputDecorator.
        /// </summary>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public UIElement Prefix
        {
            get
            {
                return _prefix;
            }
            set
            {
                if (_prefix != value)
                {
                    if (_prefix != null)
                    {
                        // notify the visual layer that the old bullet has been removed.
                        RemoveVisualChild(_prefix);

                        //need to remove old element from logical tree
                        RemoveLogicalChild(_prefix);
                    }

                    _prefix = value;

                    AddLogicalChild(value);
                    // notify the visual layer about the new child.
                    AddVisualChild(value);

                    // If we decorator content exists we need to move it at the end of the visual tree
                    UIElement child = Child;
                    if (child != null)
                    {
                        RemoveVisualChild(child);
                        AddVisualChild(child);
                    }

                    InvalidateMeasure();
                }
            }
        }

        public UIElement Suffix
        {
            get
            {
                return _suffix;
            }
            set
            {
                if (_suffix != value)
                {
                    if (_suffix != null)
                    {
                        // notify the visual layer that the old bullet has been removed.
                        RemoveVisualChild(_suffix);

                        //need to remove old element from logical tree
                        RemoveLogicalChild(_suffix);
                    }

                    _suffix = value;

                    AddLogicalChild(value);
                    // notify the visual layer about the new child.
                    AddVisualChild(value);

                    // If we decorator content exists we need to move it at the end of the visual tree
                    UIElement child = Child;
                    if (child != null)
                    {
                        RemoveVisualChild(child);
                        AddVisualChild(child);
                    }

                    InvalidateMeasure();
                }
            }
        }



        /// <summary> 
        /// Returns enumerator to logical children.
        /// </summary>
        protected override IEnumerator LogicalChildren
        {
            get
            {
                if (_prefix == null && _suffix == null)
                {
                    return base.LogicalChildren;
                }

                if (_prefix == null)
                {
                    if (Child == null)
                    {
                        return new SingleChildEnumerator(_suffix);
                    }
                    return new DoubleChildEnumerator(Child, _suffix);
                }

                if (_suffix == null)
                {
                    if (Child == null)
                    {
                        return new SingleChildEnumerator(_prefix);
                    }
                    return new DoubleChildEnumerator(Child, _prefix);
                }

                return new ThreeChildEnumerator(_prefix, Child, _suffix);
            }
        }


        /// <summary>
        /// Override from UIElement
        /// </summary>
        protected override void OnRender(DrawingContext dc)
        {
            // Draw background in rectangle inside border.
            Brush background = this.Background;
            if (background != null)
            {
                dc.DrawRectangle(background,
                                 null,
                                 new Rect(0, 0, RenderSize.Width, RenderSize.Height));
            }
        }

        /// <summary>
        /// Returns the Visual children count.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get { return (Child == null ? 0 : 1) + (_prefix == null ? 0 : 1) + (_suffix == null ? 0 : 1); }
        }

        /// <summary>
        /// Returns the child at the specified index.
        /// </summary>
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index > VisualChildrenCount - 1)
            {
                throw new ArgumentOutOfRangeException("index", index, "ArgumentOutOfRange");
            }

            if (index == 0)
            {
                if (_prefix != null) return _prefix;
                else return Child;
            }

            if (index == 1)
            {
                if (_prefix != null) return Child;
                else return _suffix;
            }

            if (index == 2)
            {
                if (_suffix == null || _prefix == null) return Child;
                else return _suffix;
            }

            return Child;
        }

        /// <summary>
        /// Updates DesiredSize of the InputDecorator. Called by parent UIElement.
        /// This is the first pass of layout.
        /// </summary>
        /// <param name="constraint">Constraint size is an "upper limit" that InputDecorator should not exceed.</param>
        /// <returns>InputDecorator' desired size.</returns>
        protected override Size MeasureOverride(Size constraint)
        {
            Size prefixSize = new Size();
            Size contentSize = new Size();
            Size suffixSize = new Size();
            UIElement prefix = Prefix;
            UIElement content = Child;
            UIElement suffix = Suffix;

            // If we have bullet we should measure it first
            if (prefix != null)
            {
                prefix.Measure(constraint);
                prefixSize = prefix.DesiredSize;
            }
            // If we have second child (content) we should measure it
            if (content != null)
            {
                Size contentConstraint = constraint;
                contentConstraint.Width = Math.Max(0.0, contentConstraint.Width - prefixSize.Width);

                content.Measure(contentConstraint);
                contentSize = content.DesiredSize;
            }
            // If we have bullet we should measure it first
            if (suffix != null)
            {
                suffix.Measure(constraint);
                suffixSize = suffix.DesiredSize;
            }

            Size desiredSize = new Size(prefixSize.Width + contentSize.Width + suffixSize.Width, Math.Max(prefixSize.Height, Math.Max(contentSize.Height, suffixSize.Height)));
            return desiredSize;
        }

        /// <summary>
        /// InputDecorator arranges its children - Bullet and Child.
        /// Bullet is aligned vertically with the center of the content's first line
        /// </summary>
        /// <param name="arrangeSize">Size that InputDecorator will assume to position children.</param>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            UIElement prefix = Prefix;
            UIElement content = Child;
            UIElement suffix = Suffix;
            double contentOffsetX = 0;

            double bulletOffsetY = 0;

            Size prefixSize = new Size();
            Size contentSize = new Size();
            Size suffixSize = new Size();

            // Arrange the bullet if exist
            if (prefix != null)
            {
                prefix.Arrange(new Rect(prefix.DesiredSize));
                prefixSize = prefix.RenderSize;

                contentOffsetX = prefixSize.Width;
            }

            // Arrange the content if exist
            if (content != null)
            {
                // Helper arranges child and may substitute a child's explicit properties for its DesiredSize.
                // The actual size the child takes up is stored in its RenderSize.
                contentSize = arrangeSize;
                if (prefix != null)
                {
                    contentSize.Width = Math.Max(content.DesiredSize.Width, arrangeSize.Width - prefix.DesiredSize.Width);
                    contentSize.Height = Math.Max(content.DesiredSize.Height, arrangeSize.Height);
                }
                content.Arrange(new Rect(contentOffsetX, 0, contentSize.Width, contentSize.Height));

                double centerY = GetFirstLineHeight(content) * 0.5d;
                bulletOffsetY += Math.Max(0d, centerY - prefixSize.Height * 0.5d);
            }

            // Arrange the bullet if exist
            if (suffix != null)
            {
                suffix.Arrange(new Rect(suffix.DesiredSize));
                suffixSize = suffix.RenderSize;

                contentOffsetX = suffixSize.Width + contentSize.Width;
            }

            // Re-Position the bullet if exist
            if (prefix != null && !DoubleUtil.IsZero(bulletOffsetY))
            {
                prefix.Arrange(new Rect(0, bulletOffsetY, prefix.DesiredSize.Width, prefix.DesiredSize.Height));
            }

            return arrangeSize;
        }

        // This method calculates the height of the first line if the element is TextBlock or FlowDocumentScrollViewer
        // Otherwise returns the element height
        private double GetFirstLineHeight(UIElement element)
        {
            // We need to find TextBlock/FlowDocumentScrollViewer if it is nested inside ContentPresenter
            // Common scenario when used in styles is that InputDecorator content is a ContentPresenter
            UIElement text = FindText(element);
            //ReadOnlyCollection<LineResult> lr = null;
            //if (text != null)
            //{
            //    TextBlock textElement = ((TextBlock)text);
            //    if (textElement.IsLayoutDataValid)
            //        lr = textElement.GetLineResults();
            //}
            //else
            //{
            //    text = FindFlowDocumentScrollViewer(element);
            //    if (text != null)
            //    {
            //        TextDocumentView tdv = ((IServiceProvider)text).GetService(typeof(ITextView)) as TextDocumentView;
            //        if (tdv != null && tdv.IsValid)
            //        {
            //            ReadOnlyCollection<ColumnResult> cr = tdv.Columns;
            //            if (cr != null && cr.Count > 0)
            //            {
            //                ColumnResult columnResult = cr[0];
            //                ReadOnlyCollection<ParagraphResult> pr = columnResult.Paragraphs;
            //                if (pr != null && pr.Count > 0)
            //                {
            //                    ContainerParagraphResult cpr = pr[0] as ContainerParagraphResult;
            //                    if (cpr != null)
            //                    {
            //                        TextParagraphResult textParagraphResult = cpr.Paragraphs[0] as TextParagraphResult;
            //                        if (textParagraphResult != null)
            //                        {
            //                            lr = textParagraphResult.Lines;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            //if (lr != null && lr.Count > 0)
            //{
            //    Point ancestorOffset = new Point();
            //    text.TransformToAncestor(element).TryTransform(ancestorOffset, out ancestorOffset);
            //    return lr[0].LayoutBox.Height + ancestorOffset.Y * 2d;
            //}

            return element.RenderSize.Height;
        }

        private TextBlock FindText(Visual root)
        {
            // Cases where the root is itself a TextBlock
            TextBlock text = root as TextBlock;
            if (text != null)
                return text;

            ContentPresenter cp = root as ContentPresenter;
            if (cp != null)
            {
                if (VisualTreeHelper.GetChildrenCount(cp) == 1)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(cp, 0);

                    // Cases where the child is a TextBlock
                    TextBlock textBlock = child as TextBlock;
                    if (textBlock == null)
                    {
                        AccessText accessText = child as AccessText;
                        if (accessText != null &&
                            VisualTreeHelper.GetChildrenCount(accessText) == 1)
                        {
                            // Cases where the child is an AccessText whose child is a TextBlock
                            textBlock = VisualTreeHelper.GetChild(accessText, 0) as TextBlock;
                        }
                    }
                    return textBlock;
                }
            }
            else
            {
                AccessText accessText = root as AccessText;
                if (accessText != null &&
                    VisualTreeHelper.GetChildrenCount(accessText) == 1)
                {
                    // Cases where the root is an AccessText whose child is a TextBlock
                    return VisualTreeHelper.GetChild(accessText, 0) as TextBlock;
                }
            }
            return null;
        }

        private FlowDocumentScrollViewer FindFlowDocumentScrollViewer(Visual root)
        {
            FlowDocumentScrollViewer text = root as FlowDocumentScrollViewer;
            if (text != null)
                return text;

            ContentPresenter cp = root as ContentPresenter;
            if (cp != null)
            {
                if (VisualTreeHelper.GetChildrenCount(cp) == 1)
                    return VisualTreeHelper.GetChild(cp, 0) as FlowDocumentScrollViewer;
            }
            return null;
        }
    }
}
