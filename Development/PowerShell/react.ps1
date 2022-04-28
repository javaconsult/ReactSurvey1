# https://www.npmjs.com/package/ 
$project="reactapp1"
npx create-react-app $project --template typescript
Set-Location $project
npm install axios
npm install react-bootstrap bootstrap 
npm install react-router
npm install react-router-bootstrap
npm install react-router-dom
npm install formik
npm install yup
npm install uuid
npm install mobx
npm install mobx-react-lite
npm install react-tooltip

npm install --save @types/bootstrap
npm install --save @types/react-router-bootstrap
npm install --save @types/uuid

npm start

# npm run build runs build from package.json
# to move build files to wwwroot of web app, include the following
# "postbuild": "move build ../WebApi1/wwwroot",
npm run build

# program.cs
# app.UseDefaultFiles(); //looks for index.html in wwwroot folder
# app.UseStaticFiles(); //serve static files from wwwroot
# launchSettings.json  "launchUrl": "",
dotnet watch run --project "WebApi1"
start-process https://localhost:7194