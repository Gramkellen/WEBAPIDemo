using Microsoft.AspNetCore.Mvc;
using TravelTeam_Final.Dtos;
using TravelTeam_Final.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TravelTeam_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _db;

        public UsersController(DataContext db)
        {
            _db = db;
        }
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            var user = from a in _db.Users
                       select a;

            return user;
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public IActionResult GetId(string id)
        {
            var user = (from a in _db.Users
                        where a.UserId == id
                        select a).SingleOrDefault();
            if (user == null)
            {
                return NotFound("不存在该用户");
            }

            return Ok(user);
        }

        [HttpGet("Select")]
        public IActionResult GetSelect([FromQuery] string? nickname, [FromQuery] int? vipgrade)
        {
            var user = from a in _db.Users
                       select a;
            if (nickname != null)
            {
                user = user.Where(a => a.NickName == nickname);
            }
            if (vipgrade != null)
            {
                user = user.Where(a => a.VipGrade == vipgrade);
            }
            if (user == null || user.Count() == 0)
            {
                return NotFound("查无此人");
            }
            return Ok(user);
        }

        // POST api/<UsersController>

        //新增新的用户：注册
        /*需要添加功能：
         *  1.利用正则表达式对Email进行检测
         *  2.对电话号码是否是全数字进行检测,还有电话号码的位数—11位
         *  3.对生日的有效性进行检验
         */

        [HttpPost]
        public string Post([FromBody] UsersDto value)
        {
            User user_ = DtoTrans(value);

            //也可以用内置的函数，自写函数可以同时方便进行新增功能的检验
            //_db.Users.Update(user_).CurrentValues.SetValues(value);

            _db.Users.Add(user_);
            _db.SaveChanges();

            return "新增资料成功!";
        }


        //更新资料
        //需要添加功能：允许部分更新（当然其实也可以不加）
        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] User value)
        {
            if (id != value.UserId)  //防止修改Id
            {
                return NotFound("ID不匹配，请查看是否输入有误！");
            }
            var user = (from a in _db.Users
                        where a.UserId == id
                        select a).SingleOrDefault();
            if (user == null)
            {
                return NotFound("没有找到该用户！");
            }
            _db.Users.Update(user).CurrentValues.SetValues(value);
            _db.SaveChanges();   //数据库业务提交
            return Ok("资料更新成功！");
        }

        // DELETE api/<UsersController>/5
        //删除资料
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var user = (from a in _db.Users
                        where a.UserId == id
                        select a).SingleOrDefault();
            if (user == null)
            {
                return NotFound("不存在该用户!");
            }
            else
            {
                _db.Users.Remove(user);
                _db.SaveChanges();
                return Ok("删除成功！");
            }
        }

        //如果不是HTTP方法最好改成Private
        private User DtoTrans(UsersDto ud)
        {
            User user_ = new User
            {
                Password = ud.Password,
                NickName = ud.NickName,
                PhoneNumber = ud.PhoneNumber,
                WechatNumber = ud.WechatNumber,
                Gender = ud.Gender,
                Location = ud.Location,
                Birthday = ud.Birthday,
                Email = ud.Email,
                HeadImageUrl = ud.HeadImageUrl,
                VipGrade = 0

            };

            return user_;

        }
    }
}
