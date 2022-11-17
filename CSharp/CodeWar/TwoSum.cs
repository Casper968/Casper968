using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casper968.CSharp.CodeWar
{
    public class Solution {
        public int[] TwoSum(int[] nums, int target) 
        {
            Console.WriteLine(string.Join(",", nums));
            double sq = Math.Sqrt(target);
            
            if (target == 0)
            {
                int indexA = Array.FindIndex(nums, y => y == 0);
                int indexB = Array.FindIndex(nums, indexA + 1, y => y == 0);
                if (indexA > -1 && indexB > -1)
                {
                    return new int [] { indexA, indexB};
                }
            }
            
            for (int i = 0; i < nums.Length; i++)
            {
                int partAvalue = nums[i];
                
                Console.WriteLine(string.Format("Part a value: {0}", partAvalue));
                
                if (partAvalue == target)
                {
                    return new int [] {i, Array.FindIndex(nums, y => y == 0)};
                }
                
                int subIndex = -1;
                bool hasMatch = false;
                int partBvalue = nums.ToList().FirstOrDefault(x => {
                    subIndex++;
                    hasMatch = x + partAvalue == target;
                    return i != subIndex && hasMatch;
                });
                
                Console.WriteLine(string.Format("Part b value: {0}", partBvalue));
                
                if (hasMatch) 
                {
                    return new int [] {i, Array.FindIndex(nums, i + 1, y => y == partBvalue)};
                }
            }
            
            return new int [] { -1, -1};
        }
    }
}