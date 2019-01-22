﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;

namespace OracleDotNetCore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                //Demo: Basic ODP.NET Core application for ASP.NET Core
                // to connect, query, and return results to a web page

                //Create a connection to Oracle			
                string conString = "User Id=hr;Password=hr;" +

                //How to connect to an Oracle DB without SQL*Net configuration file
                //  also known as tnsnames.ora.
                "Data Source=localhost:1521/XE;";

                //How to connect to an Oracle DB with a DB alias.
                //Uncomment below and comment above.
                //"Data Source=<service name alias>;";

                using (OracleConnection con = new OracleConnection(conString))
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        try
                        {
                            con.Open();
                            cmd.BindByName = true;

                            //Use the command to display employee names from 
                            // the EMPLOYEES table
                            cmd.CommandText = "select first_name from employees where department_id = :id";

                            // Assign id to the department number 50 
                            OracleParameter id = new OracleParameter("id", 50);
                            cmd.Parameters.Add(id);

                            //Execute the command and use DataReader to display the data
                            OracleDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                await context.Response.WriteAsync("Employee First Name: " + reader.GetString(0) + "\n");
                            }

                            reader.Dispose();
                        }
                        catch (Exception ex)
                        {
                            await context.Response.WriteAsync(ex.Message);
                        }
                    }
                }

            });

        }
    }
}
