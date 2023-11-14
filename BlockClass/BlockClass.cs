using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace BlockClass
{
    public class BlockDL
    {

        //区块
        public struct Block
        {
            public int Index;
            public string Timestamp;
            public string Data;
            public string Hash;
            public string PrevHash;
        }

        //主链
        public List<Block> BlockLink = new List<Block>();

        string BlockPatch = "";

        /// <summary>
        /// 创世块
        /// </summary>
        public BlockDL(string Patch)
        {
            BlockPatch = Patch;
            getBlockFullLink(Patch);
            if (BlockLink.Count <= 0)
            {
                Block godBlock = new Block();
                godBlock.Index = 0;
                godBlock.Timestamp = DateTime.Now.ToFileTime().ToString();
                godBlock.Data = "你好，我是创世块！";
                godBlock.PrevHash = "";
                godBlock.Hash = cHash(godBlock);
                //BlockLink.Add(godBlock);

                WriteBlock(godBlock, Patch + "\\god\\");
            }
        }
        /// <summary>
        /// 生成块hash
        /// </summary>
        /// <param name="thisBlock"></param>
        /// <returns></returns>
        public string cHash(Block thisBlock)
        {
            thisBlock.Hash = ""; 
            try
            {
                //生成hash
                SHA256 sha256 = new SHA256Managed();
                byte[] hashbit = sha256.ComputeHash(System.Text.Encoding.Default.GetBytes(JsonConvert.SerializeObject(thisBlock)));
                sha256.Clear();
                return BitConverter.ToString(hashbit).Replace("-", "");
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex);
            }
        }

        /// <summary>
        /// 生成块
        /// </summary>
        /// <param name="oldBlock"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Block cBlock(Block oldBlock, string data)
        {
            Block bl = new Block();
            bl.Index = oldBlock.Index + 1;
            bl.Timestamp = DateTime.Now.ToFileTime().ToString();
            bl.Data = data;
            bl.PrevHash = oldBlock.Hash;
            bl.Hash = cHash(bl);
            return bl;
        }

        /// <summary>
        /// 效验块是否有效
        /// </summary>
        /// <param name="oldBlock"></param>
        /// <param name="nowBlock"></param>
        /// <returns></returns>
        public bool checkBlock(Block oldBlock, Block nowBlock)
        {
            if (oldBlock.Index + 1 != nowBlock.Index)
                return false;
            if (oldBlock.Hash != nowBlock.PrevHash)
                return false;
            if (cHash(nowBlock) != nowBlock.Hash)
                return false;
            return true;
        }

        /// <summary>
        /// 替换链
        /// </summary>
        /// <param name="nowLink"></param>
        public void replaceLink(List<Block> nowLink)
        {
            if (nowLink.Count > BlockLink.Count)
                BlockLink = nowLink;
        }

        /// <summary>
        /// 添加块
        /// </summary>
        /// <param name="nowLink"></param>
        /// <param name="fileFloder"></param>
        public void writeBlock(Block nowLink, string fileFloder)
        {
            string patch = fileFloder + nowLink.Hash;

            if (!Directory.Exists(fileFloder))
                Directory.CreateDirectory(fileFloder);
            string blockStr = JsonConvert.SerializeObject(nowLink);
            File.WriteAllText(patch, blockStr);

            List<string> node = new List<string>();
            foreach (DirectoryInfo d in (new DirectoryInfo(BlockPatch)).GetDirectories())
            {
                node.Add(d.FullName);
            }
            for (int i = 0; i <= 5 && i < node.Count; i++)
            {
                while (1 == 1)
                {
                    Random rd = new Random();
                    int num = rd.Next(0, node.Count);
                    if (node[num] != "")
                    {
                        File.WriteAllText(node[num] + "\\" + nowLink.Hash, blockStr);
                        node[num] = "";
                        break;
                    }
                    else
                        continue;
                }
            }
            BlockLink.Add(nowLink);
        }

        /// <summary>
        /// 全链遍历
        /// </summary>
        /// <param name="blockFloder"></param>
        public void getBlockFullLink(string blockFloder)
        {
            DirectoryInfo root = new DirectoryInfo(blockFloder);
            List<string> blockFileList = new List<string>();
            foreach (DirectoryInfo d in root.GetDirectories())
            {
                foreach (string f in Directory.GetFiles(d.FullName))
                {
                    blockFileList.Add(f);
                }
            }

            if (blockFileList.Count > 0)
            {
                int i = 0;
                string nextPreHash = "";
                while (1 == 1)
                {
                    int Last = i;
                    foreach (string name in blockFileList)
                    {
                        if (File.Exists(name))
                        {
                            string data = File.ReadAllText(name);
                            Block bl = JsonConvert.DeserializeObject<Block>(data);
                            if (bl.Index == i)
                            {
                                if (bl.Index == 0)
                                {
                                    BlockLink.Clear();
                                    BlockLink.Add(bl);
                                    nextPreHash = bl.Hash;
                                    i++;
                                    break;
                                }
                                if (bl.PrevHash == nextPreHash)
                                {
                                    if (checkBlock(BlockLink[BlockLink.Count - 1], bl))
                                    {
                                        BlockLink.Add(bl);
                                        nextPreHash = bl.Hash;
                                        i++;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (Last == i)
                        break;
                }

            }

        }
    }
}
