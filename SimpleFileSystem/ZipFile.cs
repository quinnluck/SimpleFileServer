using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
 * Created by Quinn Luck for Proofpoint interview
 * Apr. 2017
 */

namespace SimpleFileSystem
{
    public class ZipFile
    {
        private string name;
        private string path;
        private int size;
        private object parent;
        private List<TextFile> text_children;
        private List<Folder> folder_children;
        private List<ZipFile> zip_children;

        /// <summary>
        /// Constructor for a fresh ZipFile.
        /// </summary>
        /// <param name="_name">Name of the new Zip file</param>
        /// <param name="_path">Path of the new Zip file</param>
        public ZipFile(string _name, string _path)
        {
            this.name = _name;
            this.path = _path;
            this.size = 0;
            text_children = new List<TextFile>();
            folder_children = new List<Folder>();
            zip_children = new List<ZipFile>();
        }

        /// <summary>
        /// Constructor for a ZipFile created from a Folder
        /// </summary>
        /// <param name="_name">Name of the existing folder</param>
        /// <param name="_path">Path of the existing folder</param>
        /// <param name="_size">Size of the existing folder</param>
        /// <param name="_content">Content of the existing folder</param>
        /// <param name="_parent">Parent path of the existing folder</param>
        public ZipFile(string _name, string _path, int _size, object _parent)
        {
            this.name = _name;
            this.path = _path;
            this.size = _size;
            this.parent = _parent;
            text_children = new List<TextFile>();
            folder_children = new List<Folder>();
            zip_children = new List<ZipFile>();
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
        // The Size property is set when a text file, folder, or zip file is added to this instance.
        public int Size
        {
            get { return this.size; }
        }
        public object Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }

        public List<TextFile> TextFiles
        {
            get { return this.text_children; }
        }
        public List<ZipFile> ZipFiles
        {
            get { return this.zip_children; }
        }
        public List<Folder> Folders
        {
            get { return this.folder_children; }
        }

        /*
         * The below add methods are for ease of use, and contribute to 
         * setting the Size of this folder instance.
         */
        public void addTextFile(TextFile file)
        {
            file.Parent = this;
            this.text_children.Add(file);
            sumSize(this.text_children, this.zip_children, this.folder_children);
        }

        public void addFolder(Folder folder)
        {
            folder.Parent = this;
            this.folder_children.Add(folder);
            sumSize(this.text_children, this.zip_children, this.folder_children);
        }

        public void addZipFile(ZipFile file)
        {
            file.Parent = this;
            this.zip_children.Add(file);
            sumSize(this.text_children, this.zip_children, this.folder_children);
        }

        /// <summary>
        /// Sums all sizes from all Zip files, Text files, and Folders contained in this instance.
        /// </summary>
        /// <param name="_textFiles">Text files contained in the Folder</param>
        /// <param name="_zipFiles">Zip files contained in the Folder</param>
        /// <param name="_folders">Folders contained in the Folder</param>
        public void sumSize(List<TextFile> _textFiles, List<ZipFile> _zipFiles, List<Folder> _folders)
        {
            int summed_size = 0;
            // adds sizes from each text file contained within this instance
            for (int i = 0; i < _textFiles.Count; i++)
            {
                summed_size += _textFiles.ElementAt<TextFile>(i).Size;
            }
            foreach (ZipFile file in _zipFiles)
            {
                summed_size += file.Size;
            }
            foreach (Folder fold in _folders)
            {
                summed_size += fold.Size;
            }
            this.size = summed_size/2;
        }

        /// <summary>
        /// Removes all children from the Text files, Zip files, and Folders contained within this instance.
        /// </summary>
        /// <param name="_textFiles">The children Text files contained in this Zip file</param>
        /// <param name="_zipFiles">The children Zip files contained in this Zip file</param>
        /// <param name="_folders">The children Folders contained in this Zip file</param>
        public void deleteChildren(List<TextFile> _textFiles, List<ZipFile> _zipFiles, List<Folder> _folders)
        {
            // delete all text files
            for (int i = 0; i < _textFiles.Count; i++)
            {
                _textFiles.RemoveAt(i);
            }
            // delete all zip files and their children
            for (int i = 0; i < _zipFiles.Count; i++)
            {
                ZipFile file = _zipFiles.ElementAt<ZipFile>(i);
                file.deleteChildren(file.TextFiles, file.ZipFiles, file.Folders);
                _zipFiles.RemoveAt(i);
            }
            // delete all folders and their children
            for (int i = 0; i < _folders.Count; i++)
            {
                Folder folder = _folders.ElementAt<Folder>(i);
                folder.deleteChildren(folder.TextFiles, folder.ZipFiles, folder.Folders);
                _folders.RemoveAt(i);
            }
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
