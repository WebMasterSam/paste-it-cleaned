import React from "react"
import { Switch, Route } from "react-router-dom"

import AccountHeader from "../views/account/Header"
import PluginHeader from "../views/plugin/Header"

interface HeaderProps {
  onDrawerToggle: () => void
}

class Header extends React.Component<HeaderProps> {
  render() {
    return (
      <React.Fragment>
        <Switch>
          <Route exact path="/">
            <AccountHeader {...this.props} />
          </Route>
          <Route path="/account">
            <AccountHeader {...this.props} />
          </Route>
          <Route path="/config">
            <PluginHeader {...this.props} />
          </Route>
        </Switch>
      </React.Fragment>
    )
  }
}

export default Header
