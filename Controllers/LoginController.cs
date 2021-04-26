using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDoListApp.Models;

namespace ToDoListApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(ToDoAppDbContext context, IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        // GET api/<LoginController>
        [HttpGet("{User_id}/{pass}")]
        public Login Login(string User_id, string pass)
        {
            Login login = new Login();
            string query = "select ID,User_id,Name,pass,role from user_info where user_id = '" + User_id +"' and pass = '"+ pass + "'";

            string sqlDataSource = _configuration.GetConnectionString("DevConnection");
            SqlDataReader rdr;

            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using (SqlCommand mycommand = new SqlCommand(query, con))
                {
                    rdr = mycommand.ExecuteReader();
                    while (rdr.Read())
                    {
                        if (rdr["ID"] != DBNull.Value)
                            login.ID = Guid.Parse(rdr["ID"].ToString());

                        if (rdr["User_id"] != DBNull.Value)
                            login.User_id = rdr["User_id"].ToString();

                        if (rdr["Name"] != DBNull.Value)
                            login.Name = rdr["Name"].ToString();

                        if (rdr["role"] != DBNull.Value)
                            login.role = rdr["role"].ToString();
                    }
                    rdr.Close();
                    con.Close();
                }
            }

            if (login.User_id != null)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ToDoListAppSecretKey"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                login.token = tokenString;
            }

            return login;
        }

    }
}
