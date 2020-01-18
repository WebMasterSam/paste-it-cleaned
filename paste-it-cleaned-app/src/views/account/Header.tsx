import React from 'react'
import Tab from '@material-ui/core/Tab'
import HeaderBase from '../../components/app/HeaderBase'
import Tabs from '@material-ui/core/Tabs/Tabs'
import { Link } from 'react-router-dom'
import { Url } from '../../shared/Urls'

interface AccountHeaderProps {
    onDrawerToggle: () => void
}

class AccountHeader extends React.Component<AccountHeaderProps> {
    render() {
        var activeTab = 0

        if (window.location.pathname.toLowerCase().startsWith(Url.Account)) {
            activeTab = 0
        } else if (window.location.pathname.toLowerCase().startsWith(Url.Billing)) {
            activeTab = 1
        }

        return (
            <HeaderBase
                {...this.props}
                title="Account"
                tabs={
                    <Tabs value={activeTab}>
                        <Tab
                            label={
                                <Link to={Url.Account} className="tab-link">
                                    Informations
                                </Link>
                            }
                        />
                        <Tab
                            label={
                                <Link to={Url.Billing} className="tab-link">
                                    Billing details
                                </Link>
                            }
                        />
                    </Tabs>
                }
            />
        )
    }
}

export default AccountHeader
