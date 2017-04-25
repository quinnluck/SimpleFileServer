using System;
using System.Text;
/*
 * Created by Quinn Luck for Proofpoint interview
 * Apr. 2017
 */

namespace SimpleFileSystem
{
    public class TextFile
    {
        private string name;
        private string path;
        private int size;
        private string content;
        private object parent;
        

        public TextFile(string _name, string _path)
        {
            this.name = _name;
            this.path = _path;
            this.content = "";
            this.size = 0;
        }

        public string Content
        {
            get { return this.content; }
            set
            {
                this.content = value;
                this.size = this.content.Length;
            }
        }
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public string Path
        {
            get { return this.path; }
            set { this.path = value; }
        }
        // This property is set when content is added to this instance.
        public int Size
        {
            get { return this.size; }
        }
        public object Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }

        /// <summary>
        /// Override function for Equals that is used for a Contains method
        /// </summary>
        /// <param name="obj">and object to compare this instance to</param>
        /// <returns>object equality in the form of a boolean</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is TextFile)
            {
                TextFile file = obj as TextFile;
                return file.Name == this.Name;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode() { return base.GetHashCode(); }
    }
}
