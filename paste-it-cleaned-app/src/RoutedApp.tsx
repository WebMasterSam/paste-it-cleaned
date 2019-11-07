import React from "react"
import { withRouter, RouteComponentProps } from "react-router-dom"
import App from "./App"

export class RoutedApp extends React.Component<RouteComponentProps> {
  render() {
    return <App />
  }
}

export default withRouter(RoutedApp)
