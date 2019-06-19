using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Builders;
using CleverCrow.Fluid.Dialogues.Nodes;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Choices.Tests {
    public class ChoiceRuntimeTest {
        public class GetValidChildNodeMethod {
            [Test]
            public void It_should_return_a_valid_child () {
                var node = A.Node.Build();
                var children = new List<INodeRuntime> { node };
                var choice = new ChoiceRuntime(children);

                var result = choice.GetValidChildNode();

                Assert.AreEqual(node, result);
            }

            [Test]
            public void It_should_not_return_an_invalid_child () {
                var node = A.Node.WithIsValid(false).Build();
                var children = new List<INodeRuntime> { node };
                var choice = new ChoiceRuntime(children);

                var result = choice.GetValidChildNode();

                Assert.IsNull(result);
            }
        }
    }
}
