using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contract
{
    // 删除区块
    public void DeleteSection(int x, int z)
    {
        // 获取哈希值
        long hash = ModifyBlock.GetHash(x, z);
        // 如果存在地形方块
        if (ModifyBlock.map.ContainsKey(hash))
        {
            // 删除地形方块
            foreach (var item in ModifyBlock.map[hash])
                ModifyBlock.DelIntoPool(item.Value);
            ModifyBlock.map.Remove(hash);
        }
        // 如果存在树木方块
        if (EqnoTree.tree.ContainsKey(hash))
        {
            // 删除树木方块
            foreach (var item in EqnoTree.tree[hash])
                foreach (var block in item.Value)
                    ModifyBlock.DelIntoPool(block);
            EqnoTree.tree.Remove(hash);
        }
        // 如果存在树木方块
        if (EqnoTree.map.ContainsKey(hash))
            ModifyBlock.map.Remove(hash);
    }
}