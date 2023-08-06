using Microsoft.AspNetCore.Mvc;
using TravelTeam_Final.Dtos;
using TravelTeam_Final.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TravelTeam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        // GET: api/<TeamController>
        private readonly DataContext _db;
        public TeamsController(DataContext db)  //注入资料
        {
            _db = db;
        }

        /*---------------------GET----------------------------*/
        //查询所有小队数据
        [HttpGet]
        public IEnumerable<Travelteam> Get()
        {
            var result = from a in _db.Travelteams
                         select a;
            return result;
        }

        //根据队伍Id查询数据
        // GET api/<TeamController>/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var team = from a in _db.Travelteams
                       where a.TeamId == id
                       select a;
            if (team == null)
                return NotFound("没有找到这支队伍！");

            return Ok(team);
        }

        //根据名字或者出行时间查询数据
        [HttpGet("Select")]
        public IActionResult GetSelect(string? destination, DateTime? travelBeginTime)
        {
            var teams = from a in _db.Travelteams
                        select a;
            if (destination != null)
            {
                teams = teams.Where(a => a.Destination == destination);
            }
            if (travelBeginTime != null)
            {
                teams = teams.Where(a => a.TravelBeginTime == travelBeginTime);
            }
            if (teams == null || teams.Count() == 0)
            {
                return NotFound("不存在这个条件的旅行小队,可自行创建！");
            }

            return Ok(teams);
        }

        //返回这个小队存在的成员
        [HttpGet("Numbers/{id}")]
        public IActionResult GetNumbers([FromRoute]string id)
        {
            //如果不存在这个队伍
            if(!_db.Travelteams.Any(a=>a.TeamId==id))
            {
                return NotFound("抱歉，不存在这个小队！");
            }
            var numbers = from a in _db.Teamrelations
                          from b in _db.Users
                          where a.TeamId == id && a.UserId == b.UserId
                          select b;
            if(numbers==null || numbers.Count() == 0)
            {
                return NotFound("小队中没有成员额！");
            }
            return Ok(numbers);

        }

        /*---------------------POST-------------------------*/
        //新增小队
        // POST api/<TeamController>
        [HttpPost]
        public IActionResult Post([FromBody]string id,[FromBody] TeamsDto value)
        {
            //不存在这个用户,创建一个小队理论上也要传递该小队长的id
            if (!_db.Users.Any(a => a.UserId == id))
            {
                return NotFound("不存在该用户，无法创建小队！");
            }
            Travelteam team = new Travelteam();
            _db.Travelteams.Add(team).CurrentValues.SetValues(value);
            //进行内置函数对应之后再进行一个修改
            team.PostTime = DateTime.Now;
            team.TeamStatus = 0;
            team.Currentnumber = 1;
            _db.SaveChanges();

            //每次新增小队，对应的队伍的联系集也要更新
            Teamrelation tl = new Teamrelation
            {
                TeamId = team.TeamId,
                UserId = id,
                UserStatus = 2
            };
            _db.Teamrelations.Add(tl);
            _db.SaveChanges();

            //如果标签不为0，对应的标签集也要更新呢
            if(value.TagList!=null && value.TagList.Count()!=0)
            {
                foreach(string t in value.TagList)
                {
                    _db.Tags.Add(new Tag
                    {
                        TeamId = team.TeamId,
                        TagName = t
                    });
                }
                _db.SaveChanges();
            }

            //如果发布内容含有图片，需要更新图片库
            if (value.MediaList != null && value.MediaList.Count() != 0)
            {
                foreach (string t in value.MediaList)
                {
                    _db.Mediamessages.Add(new Mediamessage
                    {
                        MediaUrl=t,  //传入为图片路径
                        MediaSta=0,   //0表示图片是发布内容中的
                        MteamId=team.TeamId,
                        MuserId=id,
                        PostTime=DateTime.Now   //上传时间系统定义
                    });
                }
                _db.SaveChanges();
            }

            return Ok(team);

        }

        //申请加入小队
        [HttpPost("Apply")]
        public IActionResult ApllyToTeam([FromBody] string teamid, [FromBody] string userid)
        {
            if(!_db.Users.Any(a=>a.UserId==userid))
            {
                return NotFound("不存在该用户！是否输入有误！");
            }else if(!_db.Travelteams.Any(a => a.TeamId == teamid))
            {
                return NotFound("不存在该小队！是否输入有误！");
            }

            Teamrelation rl = new Teamrelation
            {
                TeamId = teamid,
                UserId = userid,
                UserStatus = 0    //0表示申请加入
            };
            _db.Teamrelations.Add(rl);
            _db.SaveChanges();


            return Ok("申请成功！等待队长审核！");


        }


        /*---------------------PUT-------------------------*/
        /*更新小队信息，传入参数为小队id以及需要更新的信息
         * 这里不能增加新的成员，也不能修改发布时间
         */
        // PUT api/<TeamController>/5
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] string id, [FromBody] TeamsPutDto value)
        {
            var team = (from a in _db.Travelteams
                        where a.TeamId == id
                        select a).SingleOrDefault();
            
            if(team==null)
            {
                return NotFound("Id输入有误.未找到对应队伍");
            }
            //如果标签更新了,需要删除原来的标签，并更新新的标签
            if(value.TagList!=null && value.TagList.Count() > 0)
            {
                //删除
                var tags = from a in _db.Tags
                           where a.TagName == id
                           select a;
                if(tags!=null)
                {
                    foreach(var tag in tags)
                    {
                        _db.Remove(tag);
                    }
                    _db.SaveChanges(); 
                }
                //新增标签
                foreach (string t in value.TagList)
                {
                    _db.Tags.Add(new Tag
                    {
                        TeamId = team.TeamId,
                        TagName = t
                    });
                }
                _db.SaveChanges();

            }

            //如果发布图片也更新了，需要删除原来存储的图片并进行更新
            if (value.MediaList != null && value.MediaList.Count() > 0)
            {
                //删除
                var medias = from a in _db.Mediamessages
                           where a.MteamId == id
                           select a;
                if (medias != null)
                {
                    foreach (var media in medias)
                    {
                        _db.Remove(media);
                    }
                    _db.SaveChanges();
                }
                //新增图片
                foreach (string t in value.MediaList)
                {
                    _db.Mediamessages.Add(new Mediamessage
                    {
                        MediaUrl = t,  //传入为图片路径
                        MediaSta = 0,   //0表示图片是发布内容中的
                        MteamId = team.TeamId,
                        MuserId = id,
                        PostTime = DateTime.Now   //上传时间系统定义
                    });
                }
                _db.SaveChanges();

            }

            _db.Travelteams.Update(team).CurrentValues.SetValues(value);
            team.PostTime = DateTime.Now;
            _db.SaveChanges();

            return Ok("更新成功！");

        }

        //添加小队成员
        [HttpPut("Add")]
        public IActionResult AddNumber([FromBody] string teamid, [FromBody] string userid)
        {
            //进行正确性检查
            if (!_db.Users.Any(a => a.UserId == userid))
            {
                return NotFound("不存在该用户！是否输入有误！");
            }

            //从数据库中捞取队伍
            var team = (from a in _db.Travelteams
                        where a.TeamId == teamid
                        select a).SingleOrDefault();
            if(team==null)
            {
                return NotFound("不存在该小队");
            }
            //更新联系集
            var rl = (from a in _db.Teamrelations
                               where a.TeamId == teamid && a.UserId==userid
                               select a).SingleOrDefault();
            if(rl==null)
            {
                return NotFound("没有提前申请！请先提前申请！");
            }
            rl.UserStatus = 1;
            _db.Teamrelations.Add(rl);
            //更新队伍信息
            team.Currentnumber++;
            if(team.Currentnumber==team.Maxnumber)  //如果够人了，招募结束
            {
                team.TeamStatus = 1;
            }
            _db.Travelteams.Update(team);
            _db.SaveChanges();

            return Ok("添加成员成功！");

        }

        //结束小队招募
        [HttpPut("Ending")]
        public IActionResult Ending([FromBody] string teamid)
        {
            var team = (from a in _db.Travelteams
                        where a.TeamId == teamid
                        select a).SingleOrDefault();
            if (team == null)
            {
                return NotFound("没有找到该小队");
            }
            team.TeamStatus = 1;

            _db.Travelteams.Update(team);
            _db.SaveChanges();

            return Ok("结束招募成功！");
        }

        /*---------------------DELETE-------------------------*/
        // DELETE api/<TeamController>/5
        //删除对应的小队
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            //因为在数据库中设置了级联删除，所以不用额外操作
            var team = (from a in _db.Travelteams
                        where a.TeamId == id
                        select a).SingleOrDefault();
            if (team == null)
            {
                return NotFound("不存在该用户!");
            }
            else
            {
                _db.Travelteams.Remove(team);
                _db.SaveChanges();
                return Ok("删除成功！");
            }
        }
    }
}
