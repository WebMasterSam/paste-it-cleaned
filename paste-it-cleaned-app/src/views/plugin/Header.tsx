import React from "react"
import Tab from "@material-ui/core/Tab"
import HeaderBase from "../../components/HeaderBase"
import Tabs from "@material-ui/core/Tabs/Tabs"
import { Link } from "react-router-dom"
import { Url } from "../../shared/Urls"

interface PluginHeaderProps {
  onDrawerToggle: () => void
}

class PluginHeader extends React.Component<PluginHeaderProps> {
  render() {
    var activeTab = 0

    if (window.location.pathname.toLowerCase().startsWith(Url.ApiKey)) {
      activeTab = 0
    } else if (window.location.pathname.toLowerCase().startsWith(Url.PluginConfig)) {
      activeTab = 1
    } else if (window.location.pathname.toLowerCase().startsWith(Url.PluginIntegration)) {
      activeTab = 2
    }

    return (
      <HeaderBase
        {...this.props}
        title="Plugin"
        tabs={
          <Tabs value={activeTab}>
            <Tab
              label={
                <Link to={Url.ApiKey} className="tab-link">
                  Api Key(s)
                </Link>
              }
            />
            <Tab
              label={
                <Link to={Url.PluginConfig} className="tab-link">
                  Plugin configuration
                </Link>
              }
            />
            <Tab
              label={
                <Link to={Url.PluginIntegration} className="tab-link">
                  Plugin integration
                </Link>
              }
            />
          </Tabs>
        }
      />
    )
  }
}

export default PluginHeader
