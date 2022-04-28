import { Container } from "react-bootstrap";
import Image from 'react-bootstrap/Image'

export default function HomePage() {
  return (
<div className="background-image">
    <Container  >
    <div className="text-center text-white">

      {/* <div className="cover-container d-flex w-100 h-100 p-3 mx-auto flex-column"> */}
        {/* <main className="px-3"> */}
        <br/>
          <h1>Dineen Surveys</h1>
          <p className="lead">Create a survey, invite participants and view the results</p>
          <p className="lead">
            Built with React and ASP.NET Core. Hosted by Azure
          </p>
        {/* </main> */}
      {/* </div> */}
      <Image src="architecture1.png" fluid />
    </div>

      </Container>
      </div>
  )
}