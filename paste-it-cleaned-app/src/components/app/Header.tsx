import React from 'react'
import { Switch, Route } from 'react-router-dom'

import AccountHeader from '../../views/account/Header'
import PluginHeader from '../../views/plugin/Header'
import { Url } from '../../shared/Urls'
import AnalyticsHeader from '../../views/analytics/Header'
import DashboardHeader from '../../views/dashboard/Header'

interface HeaderProps {
    onDrawerToggle: () => void
}

class Header extends React.Component<HeaderProps> {
    render() {
        return (
            <React.Fragment>
                <Switch>
                    <Route exact path={Url.Dashboard}>
                        <DashboardHeader {...this.props} />
                    </Route>

                    <Route path={Url.Account}>
                        <AccountHeader {...this.props} />
                    </Route>
                    <Route path={Url.Billing}>
                        <AccountHeader {...this.props} />
                    </Route>

                    <Route path={Url.ApiKey}>
                        <PluginHeader {...this.props} />
                    </Route>
                    <Route path={Url.PluginConfig}>
                        <PluginHeader {...this.props} />
                    </Route>
                    <Route path={Url.PluginIntegration}>
                        <PluginHeader {...this.props} />
                    </Route>

                    <Route path={Url.Usage}>
                        <AnalyticsHeader {...this.props} />
                    </Route>
                    <Route path={Url.Hits}>
                        <AnalyticsHeader {...this.props} />
                    </Route>
                </Switch>
            </React.Fragment>
        )
    }
}

export default Header
