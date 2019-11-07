import React from "react"
import Tab from "@material-ui/core/Tab"
import HeaderBase from "../../components/HeaderBase"
import Tabs from "@material-ui/core/Tabs/Tabs"
import { Link } from "react-router-dom"
import { Url } from "../../shared/Urls"

interface DashboardHeaderProps {
  onDrawerToggle: () => void
}

class DashboardHeader extends React.Component<DashboardHeaderProps> {
  render() {
    /*var activeTab = 0

    if (window.location.pathname.toLowerCase().startsWith(Url.Usage)) {
      activeTab = 0
    } else if (window.location.pathname.toLowerCase().startsWith(Url.Hits)) {
      activeTab = 1
    }*/

    return <HeaderBase {...this.props} title="Dashboard" tabs={<div></div>} />
  }
}

export default DashboardHeader
