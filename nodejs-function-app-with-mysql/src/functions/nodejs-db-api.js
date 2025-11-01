const { app } = require('@azure/functions');

app.http('nodejs-db-api', {
    methods: ['GET', 'POST'],
    authLevel: 'anonymous',
    handler: async (request,context) => {
        var mysql = require('mysql2/promise');
        var jsonBody;
        var sqlconnection = mysql.createPool({
            host: process.env.conn,
            user: process.env.user,
            password: process.env.pass,
            database: process.env.db
          });         

         
              var sqlquery="SELECT CourseID,CourseName,Rating FROM Course";              
              var result = await sqlconnection.query(sqlquery);             
              return { body: JSON.stringify(result[0])};
    }
});

