import React from "react"
import ReactDOM from "react-dom"
import RoutedApp from "./RoutedApp"

import "./index.css"
import { BrowserRouter } from "react-router-dom"

ReactDOM.render(
  <BrowserRouter>
    <RoutedApp />
  </BrowserRouter>,
  document.getElementById("root")
)
