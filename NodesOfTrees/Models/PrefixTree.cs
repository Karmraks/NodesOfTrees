namespace NodesOfTrees.Models
{
    //Not for Task
    public class PrefixTree
    {
        private class TreeNodeTest
        {
            public char C { get; set; }
            public Dictionary<char, TreeNodeTest> Children { get; set; }
            public bool IsWord { get; set; }
        }

        private readonly TreeNodeTest root;

        public PrefixTree(string[] words)
        {
            root = new TreeNodeTest();
            foreach (var word in words)
            {
                AddWord(word);
            }
        }

        public bool ContainFullWord()
        {
            return true;
        }

        private void AddWord(string word)
        {
            var current = root;
            for (int i = 0; i < word.Length; i++)
            {
                if (current.Children != null && current.Children.TryGetValue(word[i], out var node))
                {
                    current = node;
                }
                else
                {
                    if (current.Children == null)
                        current.Children = new Dictionary<char, TreeNodeTest>();
                    current.Children.Add(word[i], current = new TreeNodeTest() { C = word[i] });
                }
            }

            current.IsWord = true;
        }
    }
}
