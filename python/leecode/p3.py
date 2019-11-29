class Solution:
    def lengthOfLongestSubstring(self, s: str) -> int:
        dicts= {}
        maxDis = 0
        for index, strs in enumerate(s):
            if(strs in dicts):
                distinct = index - dicts[strs] 
                if distinct > maxDis:
                    maxDis = index - dicts[strs]
                
               
            
            dicts[strs] = index
            
        return maxDis

s = Solution()
print(s.lengthOfLongestSubstring(" "))