using CleverCrow.Fluid.Dialogues.Builders;
using CleverCrow.Fluid.Dialogues.Graphs;
using NUnit.Framework;

namespace FluidDialogue.Tests.Editor {
    public class DialogueGraphTest {
        public class CloneMethod {
            [Test]
            public void It_should_return_a_new_instance () {
                var graph = new DialogueGraphInternal(A.Node.Build());

                var clone = graph.Clone();

                Assert.IsNotNull(clone);
                Assert.AreNotEqual(clone, graph);
            }

            [Test]
            public void It_should_populate_root_with_its_Clone_method () {
                var rootClone = A.Node.Build();
                var root = A.Node
                    .WithClone(rootClone)
                    .Build();
                var graph = new DialogueGraphInternal(root);

                var clone = graph.Clone();

                Assert.AreEqual(rootClone, clone.Root);
            }
        }
    }
}
