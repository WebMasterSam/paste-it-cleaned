import React from 'react'
import Tab from '@material-ui/core/Tab'
import HeaderBase from '../../components/app/HeaderBase'
import Tabs from '@material-ui/core/Tabs/Tabs'
import { Link } from 'react-router-dom'
import { Url } from '../../shared/Urls'

interface AnalyticsHeaderProps {
    onDrawerToggle: () => void
}

class AnalyticsHeader extends React.Component<AnalyticsHeaderProps> {
    render() {
        var activeTab = 0

        if (window.location.pathname.toLowerCase().startsWith(Url.Usage)) {
            activeTab = 0
        } else if (window.location.pathname.toLowerCase().startsWith(Url.Hits)) {
            activeTab = 1
        }

        return (
            <HeaderBase
                {...this.props}
                title="Analytics"
                tabs={
                    <Tabs value={activeTab}>
                        <Tab
                            label={
                                <Link to={Url.Usage} className="tab-link">
                                    Usage
                                </Link>
                            }
                        />
                        <Tab
                            label={
                                <Link to={Url.Hits} className="tab-link">
                                    Latest pastes
                                </Link>
                            }
                        />
                    </Tabs>
                }
            />
        )
    }
}

export default AnalyticsHeader
