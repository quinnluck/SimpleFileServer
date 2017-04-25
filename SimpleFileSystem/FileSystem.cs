using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * Created by Quinn Luck for Proofpoint interview
 * Apr. 2017
 */

namespace SimpleFileSystem
{
    class FileSystem
    {
        public List<TextFile> textFiles;
        public List<Folder> folders;
        public List<ZipFile> zipFiles;
        public List<Drive> drives;

        public FileSystem()
        {
            textFiles = new List<TextFile>();
            folders = new List<Folder>();
            zipFiles = new List<ZipFile>();
            drives = new List<Drive>();
        }

        /// <summary>
        /// Creates a specified type of file with a name and path.
        /// </summary>
        /// <param name="type">Case sensitive type of the new file</param>
        /// <param name="name">Specified name of the new file</param>
        /// <param name="path">Specified path of the new file</param>
        public void Create(string type, string name, string path)
        {
            // we only need to check one List, if it is null, we know there is no FileSystem active.
            if (textFiles == null)
            {
                throw new System.NullReferenceException("No File System!");
            }
            else
            {
                Type file_type = Type.GetType(type);
                if (file_type == typeof(TextFile))
                {
                    TextFile file = new TextFile(name, path);
                    if (!(textFiles.Contains(file)))
                    {
                        textFiles.Add(file);
                    }
                }
                else if (file_type == typeof(ZipFile))
                {
                    ZipFile file = new ZipFile(name, path);
                    if(!(zipFiles.Contains(file)))
                    {
                        zipFiles.Add(file);
                    }
                }
                else if (file_type == typeof(Folder))
                {
                    Folder folder = new Folder(name, path);
                    if(!(folders.Contains(folder)))
                    {
                        folders.Add(folder);
                    }
                }
                else if (file_type == typeof(Drive))
                {
                    Drive drive = new Drive(name, path);
                    if (!(drives.Contains(drive)))
                    {
                        drives.Add(drive);
                    }
                }
                else
                {
                    // we know the file type passed in is non-existant
                    throw new System.ArgumentException("Invalid File type!", "type");
                }
            }
        }

        /// <summary>
        /// Deletes an existing entity and all the entities it contains.
        /// </summary>
        /// <param name="path">path of entity to delete</param>
        public void Delete(string path)
        {
            // we only need to check one List, if it is null, we know there is no FileSystem active.
            if (textFiles == null)
            {
                throw new System.NullReferenceException("No File System!");
            }
            for (int i = 0; i < textFiles.Count; i++)
            {
                if(textFiles.ElementAt<TextFile>(i).Path.Equals(path))
                {
                    textFiles.RemoveAt(i);
                    return;
                }
            }
            // delete zip file matching path and its children
            for (int i = 0; i < zipFiles.Count; i++)
            {
                if (zipFiles.ElementAt<ZipFile>(i).Path.Equals(path))
                {
                    ZipFile file = zipFiles.ElementAt<ZipFile>(i);
                    file.deleteChildren(file.TextFiles, file.ZipFiles, file.Folders);
                    zipFiles.RemoveAt(i);
                    return;
                }
            }
            for (int i = 0; i < folders.Count; i++)
            {
                if (folders.ElementAt<Folder>(i).Path.Equals(path))
                {
                    Folder folder = folders.ElementAt<Folder>(i);
                    folder.deleteChildren(folder.TextFiles, folder.ZipFiles, folder.Folders);
                    folders.RemoveAt(i);
                    return;
                }
            }
            // if we havent returned by now, we know the path is invalid.
            throw new System.ArgumentException("No such path in File System!", "path");
        }

        /// <summary>
        /// Changes the parent of an entity
        /// </summary>
        /// <param name="src_path">path of the entity whose parent is to be changed</param>
        /// <param name="dest_path">path of the new parent</param>
        public void Move(string src_path, string dest_path)
        {
            // we only need to check one List, if it is null, we know there is no FileSystem active.
            if (textFiles == null)
            {
                throw new System.NullReferenceException("No File System!");
            }
            foreach (TextFile file in textFiles)
            {
                if (file.Parent.Equals(src_path))
                {
                    if(!(textFiles.Contains(file)))
                    {
                        file.Parent = dest_path;
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Unable to move, file with same name exists in destination path");
                        return;
                    }
                }
            }
            foreach (ZipFile file in zipFiles)
            {
                if (file.Parent.Equals(src_path))
                {
                    if(!(zipFiles.Contains(file)))
                    {
                        file.Parent = dest_path;
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Unable to move, file with same name exists in destination path");
                        return;
                    }
                }
            }
            foreach (Folder fold in folders)
            {
                if (fold.Parent.Equals(src_path))
                {
                    if (!(folders.Contains(fold)))
                    {
                        fold.Parent = dest_path;
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Unable to move, folder with same name exists in destination path");
                        return;
                    }
                }
            }
            // if we hit this, we know that the src_path is invalid.
            throw new System.ArgumentException("No such path in File System!", "src_path");
        }

        /// <summary>
        /// Changes the content of a text file.
        /// </summary>
        /// <param name="path">path of text file</param>
        /// <param name="new_content">new content to be written to text file</param>
        public void WriteToFile(string path, string new_content)
        {
            // we only need to check one List, if it is null, we know there is no FileSystem active.
            if (textFiles == null)
            {
                throw new System.NullReferenceException("No File System!");
            }
            foreach (TextFile file in textFiles)
            {
                if (file.Path.Equals(path))
                {
                    file.Content = new_content;
                    return;
                }
            }
            // if we hit this, we know that the path is invalid.
            throw new System.ArgumentException("No such path in File System!", "path");
        }

    }
}
