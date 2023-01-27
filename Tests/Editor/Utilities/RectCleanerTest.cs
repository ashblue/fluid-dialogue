using CleverCrow.Fluid.Dialogues.Editors;
using NUnit.Framework;
using UnityEngine;

namespace FluidDialogue.Tests.Editor.Utilities {
    public class RectCleanerTest {
        public class Positions {
            [Test]
            public void It_should_do_nothing_to_a_normal_rect_position () {
                var rect = new Rect(0, 0, 10, 10);

                var result = rect.FixNegativeSize();

                Assert.AreEqual(Vector2.zero, result.position);
            }

            [Test]
            public void It_should_adjust_axis_with_negative_size () {
                var rect = new Rect(0, 0, -10, -10);
                var expected = new Vector2(rect.x + rect.width, rect.y + rect.height);

                var result = rect.FixNegativeSize();

                Assert.AreEqual(expected, result.position);
            }
        }

        public class Sizes {
            [Test]
            public void It_should_do_nothing_to_a_normal_rect_size () {
                var expectedSize = new Vector2(10, 10);
                var rect = new Rect(Vector2.zero, expectedSize);

                var result = rect.FixNegativeSize();

                Assert.AreEqual(expectedSize, result.size);
            }

            [Test]
            public void It_should_reverse_negative_size () {
                var rect = new Rect(0, 0, -10, -10);
                var expectedSize = new Vector2(
                    Mathf.Abs(rect.size.x),
                    Mathf.Abs(rect.size.y));

                var result = rect.FixNegativeSize();

                Assert.AreEqual(expectedSize, result.size);
            }
        }
    }
}
