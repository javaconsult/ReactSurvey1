import { observer } from "mobx-react-lite";
import { Container, Navbar, NavDropdown } from "react-bootstrap";
import { Link } from "react-router-dom";
import { useStore } from "../state/Store";

function NavBar() {
    const { userStore } = useStore();
    return (
        // https://react-bootstrap.github.io/components/navbar/
        <Navbar collapseOnSelect expand="lg" bg="dark" variant="dark">
            <Container>
                {
                    userStore.isLoggedIn ?
                        <Navbar.Brand href="/surveyList">Surveys</Navbar.Brand>
                        :
                        <Navbar.Brand href="/">Home</Navbar.Brand>
                }
                <Navbar.Toggle aria-controls="responsive-navbar-nav" />

                <Navbar.Collapse id="responsive-navbar-nav" className="justify-content-end">
                    {userStore.isLoggedIn ?
                        <NavDropdown className="justify-content-end" title={userStore.user?.displayName + ' logged in'} id="collapsible-nav-dropdown">
                            <NavDropdown.Item href="/" onClick={userStore.logout} >Log out</NavDropdown.Item>
                        </NavDropdown>
                        :
                        <>
                            <Link className="me-3 nav-link text-white" to="/login">Login</Link>
                            <Link className="me-3 nav-link text-white" to="/register">Register</Link>
                        </>
                    }
                </Navbar.Collapse>
            </Container>
        </Navbar>
    )
}

export default observer(NavBar);
