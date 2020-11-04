using Ams.Core;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Calorie.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class RecordController : ControllerBase
    {
        [HttpGet]
        public IHttpActionResult GetRecord(string begindate, string enddate, string keyword)
        {
            try
            {
                enddate = Convert.ToDateTime(enddate).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:ss:mm");
                string strSql = string.Format(@"select convert(varchar(10),convert(datetime,t1.createtime),23) as createdate, t2.name,t1.meal,t2.calorie,t3.name username,t1.buyer as cardno from dbo.t_order t1 left join  
                    dbo.t_orderdetail t2 on t1.id = t2.id  left join dbo.usertab t3 on t1.buyer = t3.username 
                    where t1.createtime between '{0}' and '{1}'  {2} ", begindate, enddate, string.IsNullOrEmpty(keyword) ? "" :
                    string.Format(@"and ( t3.name like '%{0}%' or  t3.username like '%{0}%')", keyword));
                var list = Db.Context(APP.DB_DEFAULT_CONN_NAME).Sql(strSql).QueryMany<dynamic>();
                return Success("", list);
            }
            catch (Exception e)
            {
                return Exception("检索记录过程发生异常!");
            }
        }

        [HttpGet]
        public IHttpActionResult GetMealRecord(string begindate, string enddate, string keyword)
        {
            try
            {
                enddate = Convert.ToDateTime(enddate).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:ss:mm");
                string strSql = string.Format(@"select convert(varchar(10),convert(datetime,t1.createtime),23) as createdate, t2.name,count(1) as ordercount from dbo.t_order t1 left join  dbo.t_orderdetail t2 on t1.id = t2.id 
        left join dbo.usertab t3 on t1.buyer = t3.username where t1.createtime between '{0}' and '{1}' {2} group by convert(datetime,t1.createtime),t2.name order by t1.createtime  ", begindate, enddate,
            string.IsNullOrEmpty(keyword) ? "" : string.Format(@"and ( t3.name like '%{0}%' or  t3.username like '%{0}%')", keyword));
                var list = Db.Context(APP.DB_DEFAULT_CONN_NAME).Sql(strSql).QueryMany<dynamic>();
                return Success("", list);
            }
            catch (Exception e)
            {
                return Exception("检索记录过程发生异常!");
            }
        }

        [HttpGet]
        public IHttpActionResult GetFavMeal(string begindate, string enddate)
        {
            try
            {
                enddate = Convert.ToDateTime(enddate).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:ss:mm");
                string strSql = string.Format(@"select *from(
                            select t2.name,count(1) as ordercount from dbo.t_order t1 left join  dbo.t_orderdetail t2 on t1.id = t2.id
                            where t1.createtime between '{0}' and '{1}'
                            group by t2.name) as t order by t.ordercount desc ", begindate, enddate);
                var list = Db.Context(APP.DB_DEFAULT_CONN_NAME).Sql(strSql).QueryMany<dynamic>();
                return Success("", list);
            }
            catch (Exception e)
            {
                return Exception("检索记录过程发生异常!");
            }
        }
    }
}
