using System;

namespace listHelper
{
    class ListHelper
    {
        //accepts a list and returns a defined range
        public static string listSelector(string[] list,int x , int y,string seppareter)
        {
            string item_finish="";
            for(int i = x; i<y;i++)
            {   
                if(i == x)
                {
                    item_finish =  list[i];
                }
                else 
                {
                    item_finish = item_finish + seppareter + list[i];
                }
                
                
            }
            return item_finish;
        }
    }
}