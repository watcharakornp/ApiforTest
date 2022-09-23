using APi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APi.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
       
        [HttpPost]
        [Route("/api/min-max-multiplier")]
        public IActionResult MinMaxMultiplier(RequestParam num)
        {
            object oReturn = null;
            try
            {
                if (num.request.Count() <= 6) throw new Exception("จำนวนเลขต้องมากกว่า 6 ตัวขึ้นไป");
                
                int[] numbers = num.request.ToArray();
                var result = Array.Empty<int>();

                for (int i = 0; i < numbers.Length; i++)
                    if (i != numbers.Length - 1)
                        result = result.Append(numbers[i] * numbers[i + 1]).ToArray();

                ResponseParam response = new ResponseParam()
                {
                    min = result.Min(),
                    max = result.Max(),
                };
                oReturn = new { response };
            }
            catch (Exception ex)
            {
                return BadRequest("Message : " + ex.Message.ToString());
            }
            
            return Ok(oReturn);
        }

        [HttpPost]
        [Route("/api/max-length-repeated-of-subarray")]
        public IActionResult MaxLengthOfSubQuery(RequestParam2 request)
        {
            
            object oReturn = null;
            try
            {
                //Add Data to Dictionary
                Dictionary<int, List<int>> Dict = new Dictionary<int, List<int>>();
                Dict.Add(1, request.num1.ToList());
                Dict.Add(2, request.num2.ToList());

                var intList = new List<int[]>();
                Dictionary<int, List<int[]>> DictResult = new Dictionary<int, List<int[]>>();
                foreach (KeyValuePair<int, List<int>> entry in Dict)
                {
                    List<int[]> ints = new List<int[]>();
                    int keys = entry.Key;
                    List<int> lists = entry.Value;
                    for (int i = 0; i <= lists.Count - 1; i++)
                    {
                        for (int j = 0; j <= lists.Count - 1; j++)
                        {
                            if (j == 0)
                                ints.Add(lists.Skip(i).ToArray());
                            else
                                ints.Add(lists.Skip(i).SkipLast(j).ToArray());
                        }
                    }

                    var intsTmp = ints.GroupBy(c => string.Join(",", c)).ToList();


                    foreach (var item in intsTmp)
                    {
                        if (!string.IsNullOrEmpty(item.Key))
                        {
                            var s = item.Key?.Split(',')?.Select(Int32.Parse)?.ToList().ToList();
                            if (!DictResult.ContainsKey(keys))
                            {
                                DictResult.Add(keys, new List<int[]>());
                            }
                            DictResult[keys].Add(s.ToArray());
                        }
                    }

                }

                foreach (KeyValuePair<int, List<int[]>> entry in DictResult)
                {
                    int keys = entry.Key;
                    List<int[]> lists = entry.Value;
                    foreach (var item in lists) intList.Add(item);
                }

                var result1 = intList.GroupBy(c => String.Join(",", c)).Where(m => m.Count() > 1).Select(c => c.First().ToList()).ToList();
                int max = 0;


                foreach (var item in result1.Where(x => x.Count() > 1))
                {
                    Console.WriteLine(string.Join(',', item));
                    max = item.Count() > max ? item.Count : max;
                }

                //Response Object
                ResponseParam2 response = new ResponseParam2()
                {
                    max = max,
                };
                oReturn = new { response };
            }
            catch (Exception ex)
            {
                return BadRequest("Message : " + ex.Message.ToString());
            }
            
            return Ok(oReturn);
        }


    }
}
